using AutocompleteMenuNS;
using ScintillaNET;
using SketchIt.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace SketchIt.Controls
{
    public class EditorControl : Scintilla
    {
        private AutocompleteMenu _autocompleteMenu;
        private string _keywordList = "class extends implements import interface new case do while else if in switch throw get set function var try catch finally while with default break continue delete return each const namespace package include use is as instanceof typeof author copy default deprecated eventType example exampleText exception haxe inheritDoc internal link mtasc mxmlc param private return see serial serialData serialField since throws usage version langversion playerversion productversion dynamic private public partial static intrinsic internal native override protected AS3 final super this arguments null Infinity NaN undefined true false abstract as base bool break by byte case catch char checked class const continue decimal default delegate do double descending explicit event extern else enum false finally fixed float for foreach from goto group if implicit in int interface internal into is lock long new null namespace object operator out override orderby params private protected public readonly ref return switch struct sbyte sealed short sizeof stackalloc static string select this throw true try typeof uint ulong unchecked unsafe ushort using var virtual volatile void while where yield";

        public EditorControl()
        {
            string list = "";

            foreach (string word in _keywordList.Split(new char[] { ' ' }))
            {
                if (list.IndexOf(word + " ") > -1) continue;
                list += word + " ";
            }

            _keywordList = list.Trim();

            Lexer = Lexer.Cpp;
            BorderStyle = System.Windows.Forms.BorderStyle.None;
            UsePopup(PopupMode.All);
            AutoCIgnoreCase = true;
            AutoCMaxHeight = 15;
            AutoCOrder = Order.PerformSort;

            UpdateAppearance();

            // Enable automatic folding
            AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

            SetKeywords(0, _keywordList);
            //SetKeywords(1, "void null ArgumentError arguments Array Boolean Class Date DefinitionError Error EvalError Function int Math Namespace Number Object RangeError ReferenceError RegExp SecurityError String SyntaxError TypeError uint XML XMLList Boolean Byte Char DateTime Decimal Double Int16 Int32 Int64 IntPtr SByte Single UInt16 UInt32 UInt64 UIntPtr Void Path File System Windows Forms ScintillaNET");

            SetKeywords(1, Program.MainForm.Parser.KnownKeywords);
            SetKeywords(3, Program.MainForm.Parser.KnownTypes);

            _autocompleteMenu = new AutocompleteMenu();
            _autocompleteMenu.TargetControlWrapper = new ScintillaWrapper(this);
        }

        internal void UpdateAppearance()
        {
            CaretForeColor = AppearanceSettings.MenuTextColor;
            SetSelectionBackColor(true, Color.FromArgb(0x264F78));
            SetSelectionForeColor(true, Color.White);

            StyleResetDefault();
            string[] fonts = Settings.User.GetString("editor.fontName", "Courier New").Split(new char[] { ',' });

            foreach (string font in fonts)
            {
                string name = font.Trim().Replace("'", "").Replace("\"", "");

                try
                {
                    using (FontFamily ff = new FontFamily(name))
                    {
                        Styles[Style.Default].Font = name; //"Consolas";//"Fira Code Retina";
                        break;
                    }
                }
                catch (Exception)
                {
                }
            }

            Styles[Style.Default].Size = Settings.User.GetInt("editor.fontSize", 11);
            Styles[Style.Default].Bold = Settings.User.GetBool("editor.fontBold", false);
            Styles[Style.Default].BackColor = AppearanceSettings.EditorBackColor;
            Styles[Style.Default].ForeColor = AppearanceSettings.EditorForeColor;
            StyleClearAll();

            Styles[Style.LineNumber].BackColor = AppearanceSettings.EditorMarginBackColor;
            Styles[Style.LineNumber].ForeColor = AppearanceSettings.EditorLineNumberForeColor;

            Styles[Style.Cpp.Identifier].ForeColor = AppearanceSettings.EditorIdentifierForeColor;
            Styles[Style.Cpp.Comment].ForeColor = AppearanceSettings.EditorCommentForeColor;
            Styles[Style.Cpp.CommentLine].ForeColor = AppearanceSettings.EditorCommentLineForeColor;
            Styles[Style.Cpp.CommentDoc].ForeColor = AppearanceSettings.EditorCommentDocForeColor;
            Styles[Style.Cpp.Number].ForeColor = AppearanceSettings.EditorNumberForeColor;
            Styles[Style.Cpp.String].ForeColor = AppearanceSettings.EditorStringForeColor;
            Styles[Style.Cpp.Character].ForeColor = AppearanceSettings.EditorCharacterForeColor;
            Styles[Style.Cpp.Preprocessor].ForeColor = AppearanceSettings.EditorPreprocessorForeColor;
            Styles[Style.Cpp.Operator].ForeColor = AppearanceSettings.EditorOperatorForeColor;
            Styles[Style.Cpp.Regex].ForeColor = AppearanceSettings.EditorRegexForeColor;
            Styles[Style.Cpp.CommentLineDoc].ForeColor = AppearanceSettings.EditorCommentLineDocForeColor;
            Styles[Style.Cpp.Word].ForeColor = AppearanceSettings.EditorWordForeColor;
            Styles[Style.Cpp.Word2].ForeColor = AppearanceSettings.EditorWord2ForeColor;
            Styles[Style.Cpp.CommentDocKeyword].ForeColor = AppearanceSettings.EditorCommentDocKeyWordForeColor;
            Styles[Style.Cpp.CommentDocKeywordError].ForeColor = AppearanceSettings.EditorCommentDocKeywordErrorForeColor;
            Styles[Style.Cpp.GlobalClass].ForeColor = AppearanceSettings.EditorGlobalClassForeColor;
            Styles[Style.Cpp.Identifier].Bold = AppearanceSettings.EditorIdentifierFontBold;
            Styles[Style.Cpp.Comment].Bold = AppearanceSettings.EditorCommentFontBold;
            Styles[Style.Cpp.CommentLine].Bold = AppearanceSettings.EditorCommentLineFontBold;
            Styles[Style.Cpp.CommentLine].Italic = true;
            Styles[Style.Cpp.CommentDoc].Bold = AppearanceSettings.EditorCommentDocFontBold;
            Styles[Style.Cpp.Number].Bold = AppearanceSettings.EditorNumberFontBold;
            Styles[Style.Cpp.String].Bold = AppearanceSettings.EditorStringFontBold;
            Styles[Style.Cpp.Character].Bold = AppearanceSettings.EditorCharacterFontBold;
            Styles[Style.Cpp.Preprocessor].Bold = AppearanceSettings.EditorPreprocessorFontBold;
            Styles[Style.Cpp.Operator].Bold = AppearanceSettings.EditorOperatorFontBold;
            Styles[Style.Cpp.Regex].Bold = AppearanceSettings.EditorRegexFontBold;
            Styles[Style.Cpp.CommentLineDoc].Bold = AppearanceSettings.EditorCommentLineDocFontBold;
            Styles[Style.Cpp.Word].Bold = AppearanceSettings.EditorWordFontBold;
            Styles[Style.Cpp.Word2].Bold = AppearanceSettings.EditorWord2FontBold;
            Styles[Style.Cpp.CommentDocKeyword].Bold = AppearanceSettings.EditorCommentDocKeyWordFontBold;
            Styles[Style.Cpp.CommentDocKeywordError].Bold = AppearanceSettings.EditorCommentDocKeywordErrorFontBold;
            Styles[Style.Cpp.GlobalClass].Bold = AppearanceSettings.EditorGlobalClassFontBold;

            Zoom = Settings.User.GetInt("editor.zoom", 1);
            UpdateLineMargin();

            SetProperty("fold", "1");
            SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            Margins[1].Type = MarginType.Symbol;
            Margins[1].Mask = Marker.MaskFolders;
            Margins[1].Sensitive = true;
            Margins[1].Width = 13;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                Markers[i].SetForeColor(AppearanceSettings.EditorMarkersForeColor);
                Markers[i].SetBackColor(AppearanceSettings.EditorMarkersBackColor);
            }

            // Configure folding markers with respective symbols
            Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            SetFoldMarginColor(true, AppearanceSettings.EditorMarginBackColor);
            SetFoldMarginHighlightColor(true, AppearanceSettings.EditorBackColor);
        }

        protected override void OnZoomChanged(EventArgs e)
        {
            base.OnZoomChanged(e);
            Settings.User["editor.zoom"] = Zoom;
            UpdateLineMargin();
        }

        protected override void OnCharAdded(CharAddedEventArgs e)
        {
            base.OnCharAdded(e);

            switch (e.Char)
            {
                case 13:
                    string prevLine = Lines[CurrentLine - 1].Text.Replace("\t", new string(' ', 4)).TrimEnd();

                    if (prevLine.Length == 0)
                    {
                        prevLine = Lines[CurrentLine - 1].Text.Replace("\t", new string(' ', 4));
                    }

                    int indent = (prevLine.Length - prevLine.TrimStart().Length) / 4;
                    AddText(new string('\t', indent));

                    if (prevLine.EndsWith("{"))
                    {
                        AddText(new string('\t', 1));
                    }
                    break;

                case '.':
                    string[] members = Program.Parser.GetMembers(CurrentPosition - 1);

                    if (members.Length > 0)
                    {
                        string list = " ";

                        foreach (string member in members)
                        {
                            if (list.IndexOf(" " + member + " ") == -1)
                            {
                                list += member + " ";
                            }
                        }

                        AutoCShow(0, list.Trim());
                    }
                    break;

                case '(':
                    string word = GetWordFromPosition(CurrentPosition - 1);
                    string pre = GetWordFromPosition(CurrentPosition - 1 - word.Length - 1);

                    switch (pre)
                    {
                        case "new":
                            break;
                    }
                    break;

                case '}':
                    string currLine = Lines[CurrentLine].Text.Replace("\t", new string(' ', 4)).TrimEnd();
                    string foldParent = Lines[Lines[CurrentLine].FoldParent].Text.Replace("\t", new string(' ', 4)).TrimEnd();
                    int indentParent = (foldParent.Length - foldParent.TrimStart().Length) / 4;
                    int indentCurrent = (currLine.Length - currLine.TrimStart().Length) / 4;

                    if (indentParent != indentCurrent)
                    {
                        DeleteRange(Lines[CurrentLine].Position, (GetTextRange(CurrentPosition - 2, 1) == "\t" ? 1 : 4) * (indentCurrent - indentParent));
                    }
                    break;

                default:
                    int pos = CurrentPosition;
                    string wrd = GetWordFromPosition(pos).ToLower();
                    string lst = " ";

                    foreach (string keyword in _keywordList.Split(new char[] { ' ' }))
                    {
                        if (keyword.ToLower().Contains(wrd))
                        {
                            if (lst.IndexOf(" " + keyword + " ") >= 0) continue;
                            lst += keyword + " ";
                        }
                    }

                    if (char.IsLetter((char)e.Char))
                    {
                        foreach (string name in Program.Parser.GetTypes())
                        {
                            if (lst.IndexOf(name + " ") == -1)
                            {
                                lst += name + " ";
                            }
                        }

                        foreach (string name in Program.Parser.GetMembers(CurrentPosition))
                        {
                            if (lst.IndexOf(name + " ") == -1)
                            {
                                lst += name + " ";
                            }
                        }

                        foreach (Type type in typeof(Api.Sketch).Assembly.GetTypes())
                        {
                            string name = type.Name;

                            if (type.IsPublic && name.ToLower().Contains(wrd) && lst.IndexOf(" " + name + " ") == -1)
                            {
                                lst += name + " ";
                            }

                            foreach (MemberInfo member in type.GetMembers())
                            {
                                name = member.Name;

                                if (!name.ToLower().Contains(wrd))
                                {
                                    continue;
                                }

                                if (member is FieldInfo && ((FieldInfo)member).IsSpecialName)
                                {
                                    continue;
                                }

                                if (member is ConstructorInfo)
                                {
                                    continue;
                                }

                                if (member is MethodInfo)
                                {
                                    if (((MethodInfo)member).IsSpecialName /*|| ((MethodInfo)member).IsHideBySig*/)
                                    {
                                        name = "";
                                    }
                                }

                                if (name.Length == 0)
                                {
                                    continue;
                                }

                                if (lst.IndexOf(" " + name + " ") == -1)
                                {
                                    lst += name + " ";
                                }

                                if (lst.Contains("noiseDetail")) lst.ToString();
                            }
                        }

                        if (lst.Length > 1)
                        {
                            lst = lst.Substring(1, lst.Length - 2);
                        }

                        AutoCShow(wrd.Length, lst);
                        //CallTipShow(pos, "This is something\r\nThat can be *used*");
                    }
                    break;
            }

            UpdateLineMargin();
        }

        private void UpdateLineMargin()
        {
            Margins[0].Type = MarginType.Number;
            Margins[0].Width = Math.Max((Zoom + 11) * 3, Lines.Count.ToString().Length * (Zoom + 11));
        }
    }

    internal class DynamicCollection : IEnumerable<AutocompleteItem>
    {
        private string[] _words;

        public DynamicCollection(string[] words)
        {
            _words = words;
        }

        public IEnumerator<AutocompleteItem> GetEnumerator()
        {
            return BuildList().GetEnumerator();
        }

        private IEnumerable<AutocompleteItem> BuildList()
        {
            //find all words of the text
            var words = new Dictionary<string, string>();
            foreach (string word in _words)
                words[word] = word;

            //return autocomplete items
            foreach (var word in words.Keys)
            {
                AutocompleteItem item = new AutocompleteItem(word);
                item.ToolTipTitle = word;
                item.ToolTipText = "This is something";
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
