using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace SketchIt.Utilities
{
    public class SimpleParser
    {
        public SyntaxTree SyntaxTree { get; private set; }
        public TimeSpan ParseTime { get; private set; }

        private CSharpCompilation _compilation;
        private SemanticModel _semanticModel;
        private bool _parsing = false;

        public SimpleParser()
        {
            PopulateKnownData();
            Parse();
        }

        public string KnownKeywords
        {
            get;
            private set;
        }

        public string KnownTypes
        {
            get;
            private set;
        }

        public string KnownProperties
        {
            get;
            private set;
        }

        public string ParsedTypes
        {
            get;
            private set;
        }

        public string ParsedKeywords
        {
            get;
            private set;
        }

        private void PopulateKnownData()
        {
            KnownKeywords = " ";
            KnownProperties = " ";
            KnownTypes = " ";

            foreach (Type type in typeof(Api.Sketch).Assembly.GetTypes())
            {
                if (KnownTypes.IndexOf(" " + type.Name + " ") == -1)
                {
                    KnownTypes += KnownTypes.Length == 0 ? "" : " ";
                    KnownTypes += type.Name;
                }

                //foreach (MethodInfo method in type.GetMethods())
                //{
                //    if (method.IsPublic)
                //    {
                //        KnownKeywords += KnownKeywords.Length == 0 ? "" : " ";
                //        KnownKeywords += method.Name;
                //    }
                //}

                foreach (MemberInfo member in type.GetMembers())
                {
                    if (KnownKeywords.IndexOf(" " + member.Name + " ") == -1)
                    {
                        KnownKeywords += KnownKeywords.Length == 0 ? "" : " ";
                        KnownKeywords += member.Name;
                    }
                }

                foreach (PropertyInfo property in type.GetProperties())
                {
                    //if (property.IsPublic)
                    {
                        KnownProperties += KnownProperties.Length == 0 ? "" : " ";
                        KnownProperties += property.Name;
                    }
                }
            }

            KnownTypes = KnownTypes.Trim();
            KnownKeywords = KnownKeywords.Trim();
            KnownProperties = KnownProperties.Trim();
        }

        public void Parse()
        {
            using (Status.Set("Please wait..."))
            {
                if (_parsing) return;

                ParameterizedThreadStart threadStart = new ParameterizedThreadStart(DoParse);
                Thread thread = new Thread(threadStart);

                string text = "";

                foreach (BaseForm form in Application.OpenForms)
                {
                    if (form.Type != WindowType.SourceFile) continue;
                    text += ((EditorForm)form).EditorText + Environment.NewLine;
                }

                thread.Start(text);
            }
        }

        public SyntaxToken GetToken(int position)
        {
            if (SyntaxTree != null)
            {
                if (SyntaxTree.Length >= position)
                {
                    return SyntaxTree.GetCompilationUnitRoot().FindToken(position);
                }
            }

            return new SyntaxToken();
        }

        public SyntaxNode GetNode(int position)
        {
            try
            {
                if (SyntaxTree != null)
                {
                    if (SyntaxTree.Length >= position)
                    {
                        return SyntaxTree.GetCompilationUnitRoot().FindNode(new TextSpan(position, 1));
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public ImmutableArray<ISymbol> GetSymbols(int position)
        {
            if (_semanticModel != null && SyntaxTree.Length >= position)
            {
                SyntaxToken token = GetToken(position);
                if (token.ValueText == ".") token = token.GetPreviousToken();
                ImmutableArray<ISymbol> result = _semanticModel.LookupSymbols(position, null, null);
                return result;
            }

            return new ImmutableArray<ISymbol>();
        }

        public string[] GetMembers(int position)
        {
            List<string> result = new List<string>();
            SyntaxNode node = GetNode(position);
            ImmutableArray<ISymbol> symbols = GetSymbols(position);
            string typeName = "";
            string nodeValue = (node?.ToString() ?? "").Trim();

            if (nodeValue.EndsWith("."))
            {
                nodeValue = nodeValue.Substring(0, nodeValue.Length - 1);
            }

            if (symbols != null && symbols.Length > 0)
            {
                foreach (ISymbol symbol in symbols)
                {
                    if (symbol.Name == nodeValue)
                    {
                        if (symbol is ILocalSymbol localSymbol) typeName = localSymbol.Type.Name;
                        else if (symbol is IFieldSymbol fieldSymbol) typeName = fieldSymbol.Type.Name;
                    }
                }
            }

            if (string.IsNullOrEmpty(typeName))
            {
                foreach (SyntaxNode memberNode in SyntaxTree.GetCompilationUnitRoot().DescendantNodes())
                {
                    if (memberNode is FieldDeclarationSyntax fieldDef)
                    {
                        result.Add(fieldDef.Declaration.Variables[0].Identifier.ValueText);
                    }
                    else if (memberNode is PropertyDeclarationSyntax propertyDef)
                    {
                        result.Add(propertyDef.Identifier.ValueText);
                    }
                    else if (memberNode is MethodDeclarationSyntax methodDef)
                    {
                        result.Add(methodDef.Identifier.ValueText);
                    }
                    else if (memberNode is VariableDeclarationSyntax variableDef)
                    {
                        result.Add(variableDef.Variables[0].Identifier.ValueText);
                    }
                }
            }
            else
            {
                string[] names = node.ToString().Trim().Split(new char[] { '.' });

                foreach (SyntaxNode child in SyntaxTree.GetCompilationUnitRoot().DescendantNodes())
                {
                    if (child is ClassDeclarationSyntax classDeclaration && classDeclaration.Identifier.ValueText == typeName)
                    {
                        foreach (MemberDeclarationSyntax member in classDeclaration.Members)
                        {
                            if (member is FieldDeclarationSyntax fieldDeclaration) result.Add(fieldDeclaration.Declaration.Variables[0].Identifier.ToString());
                            if (member is MethodDeclarationSyntax methodDeclaration) result.Add(methodDeclaration.Identifier.ToString());
                            if (member is PropertyDeclarationSyntax propertyDeclaration) result.Add(propertyDeclaration.Identifier.ToString());
                        }
                    }
                }

                if (result.Count == 0)
                {
                    Type type = GetTypeFromName(typeName);

                    if (type != null)
                    {
                        foreach (FieldInfo member in type.GetFields())
                        {
                            if (member.IsSpecialName) continue;
                            result.Add(member.Name);
                        }

                        foreach (PropertyInfo member in type.GetProperties())
                        {
                            if (member.IsSpecialName) continue;
                            result.Add(member.Name);
                        }

                        foreach (MethodInfo member in type.GetMethods())
                        {
                            if (member.IsSpecialName) continue;
                            result.Add(member.Name);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        private void DoParse(object input)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                CSharpParseOptions options = new CSharpParseOptions(LanguageVersion.Default, DocumentationMode.None, SourceCodeKind.Script);

                SyntaxTree = CSharpSyntaxTree.ParseText(input.ToString(), options);
                _compilation = CSharpCompilation.Create("Sketch", new SyntaxTree[] { SyntaxTree });
                _semanticModel = _compilation.GetSemanticModel(SyntaxTree, true);

                ParsedTypes = "";
                ParsedKeywords = "";

                foreach (SyntaxNode node in SyntaxTree.GetCompilationUnitRoot().DescendantNodes())
                {
                    if (node is MethodDeclarationSyntax method)
                    {
                        ParsedKeywords += method.Identifier.ValueText + " ";
                    }
                    else if (node is FieldDeclarationSyntax field)
                    {
                        //ParsedKeywords += field.Declaration.Variables[0].Identifier.ValueText + " ";
                    }
                    else if (node is ClassDeclarationSyntax cls)
                    {
                        ParsedTypes += cls.Identifier.ValueText + " ";
                    }
                    else
                    {
                        node.ToString();
                    }
                }

                for (int i = 0; i < Application.OpenForms.Count; i++)
                {
                    if (!(Application.OpenForms[i] is BaseForm form) || form.Type != WindowType.SourceFile) continue;
                    form.Invoke(new MethodInvoker(((EditorForm)form).UpdateEditor));
                }

                ParseTime = DateTime.Now.Subtract(startTime);
            }
            catch
            {
            }
        }

        public string[] GetMembers(string typeName)
        {
            List<string> result = new List<string>();
            bool found = false;

            foreach (SyntaxNode node in SyntaxTree.GetCompilationUnitRoot().DescendantNodes())
            {
                if (node is ClassDeclarationSyntax classDef && classDef.Identifier.ValueText.Equals(typeName))
                {
                    found = true;

                    foreach (SyntaxNode memberNode in classDef.DescendantNodes())
                    {
                        if (memberNode is FieldDeclarationSyntax fieldDef)
                        {
                            result.Add(fieldDef.Declaration.Variables[0].Identifier.ValueText);
                        }
                        else if (memberNode is PropertyDeclarationSyntax propertyDef)
                        {
                            result.Add(propertyDef.Identifier.ValueText);
                        }
                        else if (memberNode is MethodDeclarationSyntax methodDef)
                        {
                            result.Add(methodDef.Identifier.ValueText);
                        }
                        else if (memberNode is VariableDeclarationSyntax variableDef)
                        {
                            result.Add(variableDef.Variables[0].Identifier.ValueText);
                        }
                    }

                    break;
                }
            }

            if (!found)
            {
                foreach (Type type in typeof(Api.Sketch).Assembly.GetTypes())
                {
                    if (type.Name.Equals(typeName))
                    {
                        found = true;

                        foreach (MemberInfo member in type.GetMembers())
                        {
                            result.Add(member.Name);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        public bool IsStatic()
        {
            if (SyntaxTree != null)
            {
                foreach (SyntaxNode node in SyntaxTree.GetCompilationUnitRoot().DescendantNodes())
                {
                    if (node is MethodDeclarationSyntax || node is ClassDeclarationSyntax)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Type GetTypeFromName(string typeName)
        {
            foreach (Type type in typeof(Api.Sketch).Assembly.GetTypes())
            {
                if (type.Name.Equals(typeName))
                {
                    return type;
                }
            }

            return null;
        }

        public string[] GetTypes()
        {
            List<string> types = new List<string>();

            foreach (SyntaxNode node in SyntaxTree.GetCompilationUnitRoot().DescendantNodes())
            {
                if (node is ClassDeclarationSyntax classDef)
                {
                    types.Add(classDef.Identifier.ValueText);
                }
            }

            return types.ToArray();
        }
    }
}
