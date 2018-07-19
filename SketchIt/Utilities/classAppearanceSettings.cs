using System.Drawing;

namespace SketchIt.Utilities
{
    public static class AppearanceSettings
    {
        public static Color BaseBackColor = Settings.User.GetColor("baseBackColor", Color.FromArgb(30, 30, 30));
        public static Color BaseForeColor = Settings.User.GetColor("baseForeColor", Color.FromArgb(200, 200, 200));
        public static Color MenuBackColor = Settings.User.GetColor("menuBackColor", Color.FromArgb(45, 45, 48));
        public static Color MenuTextColor = Settings.User.GetColor("menuTextColor", Color.White);
        public static Color ApplicationBackColor = Settings.User.GetColor("applicationBackColor", Color.FromArgb(45, 45, 48));
        public static Color ApplicationTextColor = Settings.User.GetColor("applicationTextColor", Color.White);
        public static Color StatusDefaultBackColor = Settings.User.GetColor("statusDefaultBackColor", Color.FromArgb(0, 122, 204));
        public static Color StatusDefaultForeColor = Settings.User.GetColor("statusDefaultForeColor", Color.White);
        public static Color BorderColor = Settings.User.GetColor("borderColor", Color.FromArgb(51, 51, 55));
        public static Color HighlightBackColor = Settings.User.GetColor("highlightBackColor", Color.FromArgb(51, 51, 52));
        public static Color ActiveCaptionBackColor = Settings.User.GetColor("activeCaptionBackColor", Color.FromArgb(0, 122, 204));
        public static Color ActiveCaptionTextColor = Settings.User.GetColor("activeCaptionTextColor", Color.FromArgb(255, 255, 255));
        public static Color SelectedItemBorderColor = Settings.User.GetColor("hoverItemBorderColor", Color.Transparent);
        public static Color HoverItemBorderColor = Settings.User.GetColor("hoverItemBorderColor", Color.Transparent);
        public static Color HoverItemBackColor = Settings.User.GetColor("hoverItemBackColor", Color.FromArgb(62, 62, 64));
        public static Color HeaderBackColor = Settings.User.GetColor("headerBackColor", MenuBackColor);
        public static Color HeaderForeColor = Settings.User.GetColor("headerForeColor", MenuTextColor);
        public static Color TextBoxBackColor = Settings.User.GetColor("textboxBackColor", Color.FromArgb(62, 62, 64));
        public static Color TextBoxForeColor = Settings.User.GetColor("textboxForeColor", Color.FromArgb(200, 200, 200));

        public static Color EditorBackColor = Settings.User.GetColor("editor.backColor", Color.FromArgb(0x1E1E1E));
        public static Color EditorForeColor = Settings.User.GetColor("editor.foreColor", Color.FromArgb(0xFFFFFF));
        public static Color EditorMarginBackColor = Settings.User.GetColor("editor.marginBackColor", EditorBackColor);
        public static Color EditorLineNumberForeColor = Settings.User.GetColor("editor.lineNumberForeColor", Color.FromArgb(0x2B91AF));
        public static Color EditorIdentifierForeColor = Settings.User.GetColor("editor.identifierForeColor", Color.Silver); //Color.FromArgb(0xD0DAE2);
        public static Color EditorCommentForeColor = Settings.User.GetColor("editor.commentForeColor", Color.FromArgb(0xBD758B));
        public static Color EditorCommentLineForeColor = Settings.User.GetColor("editor.commentLineForeColor", Color.FromArgb(0x40BF57));
        public static Color EditorCommentDocForeColor = Settings.User.GetColor("editor.commentDocForeColor", Color.FromArgb(0x2FAE35));
        public static Color EditorNumberForeColor = Settings.User.GetColor("editor.numberForeColor", Color.FromArgb(0xAFCE89));
        public static Color EditorStringForeColor = Settings.User.GetColor("editor.stringForeColor", Color.FromArgb(0xCA905D));
        public static Color EditorCharacterForeColor = Settings.User.GetColor("editor.characterForeColor", Color.FromArgb(0xE95454));
        public static Color EditorPreprocessorForeColor = Settings.User.GetColor("editor.preprocessorForeColor", Color.Red); //Color.FromArgb(0x8AAFEE);
        public static Color EditorOperatorForeColor = Settings.User.GetColor("editor.operatorForeColor", Color.FromArgb(0xE0E0E0));
        public static Color EditorRegexForeColor = Settings.User.GetColor("editor.regexForeColor", Color.FromArgb(0xFF00FF));
        public static Color EditorCommentLineDocForeColor = Settings.User.GetColor("editor.commentLineDocForeColor", Color.FromArgb(0x77A7DB));
        public static Color EditorWordForeColor = Settings.User.GetColor("editor.wordForeColor", Color.FromArgb(0x48A8EE));
        public static Color EditorWord2ForeColor = Settings.User.GetColor("editor.word2ForeColor", Color.Teal); //Color.FromArgb(0xF98906);
        public static Color EditorCommentDocKeyWordForeColor = Settings.User.GetColor("editor.commentDocKeywordForeColor", Color.FromArgb(0xB3D991));
        public static Color EditorCommentDocKeywordErrorForeColor = Settings.User.GetColor("editor.commentDocKeywordErrorForeColor", Color.FromArgb(0xFF0000));
        public static Color EditorGlobalClassForeColor = Settings.User.GetColor("editor.globalClassForeColor", Color.FromArgb(100, 100, 200));//Color.FromArgb(0x48A8EE);
        public static Color EditorMarkersForeColor = Settings.User.GetColor("editor.markersForeColor", EditorMarginBackColor);
        public static Color EditorMarkersBackColor = Settings.User.GetColor("editor.markersBackColor", Color.Silver);

        public static bool EditorFontBold = Settings.User.GetBool("editor.fontBold", false);
        public static bool EditorLineNumberFontBold = Settings.User.GetBool("editor.lineNumberFontBold", EditorFontBold);
        public static bool EditorIdentifierFontBold = Settings.User.GetBool("editor.identifierFontBold", EditorFontBold);
        public static bool EditorCommentFontBold = Settings.User.GetBool("editor.commentFontBold", EditorFontBold);
        public static bool EditorCommentLineFontBold = Settings.User.GetBool("editor.commentLineFontBold", EditorFontBold);
        public static bool EditorCommentDocFontBold = Settings.User.GetBool("editor.commentDocFontBold", EditorFontBold);
        public static bool EditorNumberFontBold = Settings.User.GetBool("editor.numberFontBold", EditorFontBold);
        public static bool EditorStringFontBold = Settings.User.GetBool("editor.stringFontBold", EditorFontBold);
        public static bool EditorCharacterFontBold = Settings.User.GetBool("editor.characterFontBold", EditorFontBold);
        public static bool EditorPreprocessorFontBold = Settings.User.GetBool("editor.preprocessorFontBold", EditorFontBold);
        public static bool EditorOperatorFontBold = Settings.User.GetBool("editor.operatorFontBold", EditorFontBold);
        public static bool EditorRegexFontBold = Settings.User.GetBool("editor.regexFontBold", EditorFontBold);
        public static bool EditorCommentLineDocFontBold = Settings.User.GetBool("editor.commentLineDocFontBold", EditorFontBold);
        public static bool EditorWordFontBold = Settings.User.GetBool("editor.wordFontBold", EditorFontBold);
        public static bool EditorWord2FontBold = Settings.User.GetBool("editor.word2FontBold", EditorFontBold);
        public static bool EditorCommentDocKeyWordFontBold = Settings.User.GetBool("editor.commentDocKeywordFontBold", EditorFontBold);
        public static bool EditorCommentDocKeywordErrorFontBold = Settings.User.GetBool("editor.commentDocKeywordErrorFontBold", EditorFontBold);
        public static bool EditorGlobalClassFontBold = Settings.User.GetBool("editor.globalClassFontBold", EditorFontBold);
        public static bool EditorMarkersFontBold = Settings.User.GetBool("editor.markersFontBold", EditorFontBold);

        internal static void Update()
        {
            BaseBackColor = Settings.User.GetColor("baseBackColor", Color.FromArgb(30, 30, 30));
            BaseForeColor = Settings.User.GetColor("baseForeColor", Color.FromArgb(200, 200, 200));
            MenuBackColor = Settings.User.GetColor("menuBackColor", Color.FromArgb(45, 45, 48));
            MenuTextColor = Settings.User.GetColor("menuTextColor", Color.White);
            ApplicationBackColor = Settings.User.GetColor("applicationBackColor", Color.FromArgb(45, 45, 48));
            ApplicationTextColor = Settings.User.GetColor("applicationTextColor", Color.White);
            StatusDefaultBackColor = Settings.User.GetColor("statusDefaultBackColor", Color.FromArgb(0, 122, 204));
            StatusDefaultForeColor = Settings.User.GetColor("statusDefaultForeColor", Color.White);
            BorderColor = Settings.User.GetColor("borderColor", Color.FromArgb(51, 51, 55));
            HighlightBackColor = Settings.User.GetColor("highlightBackColor", Color.FromArgb(51, 51, 52));
            ActiveCaptionBackColor = Settings.User.GetColor("activeCaptionBackColor", Color.FromArgb(0, 122, 204));
            ActiveCaptionTextColor = Settings.User.GetColor("activeCaptiontextcolor", Color.FromArgb(255, 255, 255));
            SelectedItemBorderColor = Settings.User.GetColor("hoverItemBorderColor", Color.Transparent);
            HoverItemBorderColor = Settings.User.GetColor("hoverItemBorderColor", Color.Transparent);
            HoverItemBackColor = Settings.User.GetColor("hoveritemBackColor", Color.FromArgb(62, 62, 64));
            HeaderBackColor = Settings.User.GetColor("headerBackColor", MenuBackColor);
            HeaderForeColor = Settings.User.GetColor("headerForeColor", MenuTextColor);
            TextBoxBackColor = Settings.User.GetColor("textboxBackColor", Color.FromArgb(62, 62, 64));
            TextBoxForeColor = Settings.User.GetColor("textboxForeColor", Color.FromArgb(200, 200, 200));

            EditorBackColor = Settings.User.GetColor("editor.backColor", Color.FromArgb(0x1E1E1E));
            EditorForeColor = Settings.User.GetColor("editor.foreColor", Color.FromArgb(0xFFFFFF));
            EditorMarginBackColor = Settings.User.GetColor("editor.marginBackColor", EditorBackColor);
            EditorLineNumberForeColor = Settings.User.GetColor("editor.lineNumberForeColor", Color.FromArgb(0x2B91AF));
            EditorIdentifierForeColor = Settings.User.GetColor("editor.identifierForeColor", Color.Silver); //Color.FromArgb(0xD0DAE2);
            EditorCommentForeColor = Settings.User.GetColor("editor.commentForeColor", Color.FromArgb(0xBD758B));
            EditorCommentLineForeColor = Settings.User.GetColor("editor.commentLineForeColor", Color.FromArgb(0x40BF57));
            EditorCommentDocForeColor = Settings.User.GetColor("editor.commentDocForeColor", Color.FromArgb(0x2FAE35));
            EditorNumberForeColor = Settings.User.GetColor("editor.numberForeColor", Color.FromArgb(0xAFCE89));
            EditorStringForeColor = Settings.User.GetColor("editor.stringForeColor", Color.FromArgb(0xCA905D));
            EditorCharacterForeColor = Settings.User.GetColor("editor.characterForeColor", Color.FromArgb(0xE95454));
            EditorPreprocessorForeColor = Settings.User.GetColor("editor.preprocessorForeColor", Color.Red); //Color.FromArgb(0x8AAFEE);
            EditorOperatorForeColor = Settings.User.GetColor("editor.operatorForeColor", Color.FromArgb(0xE0E0E0));
            EditorRegexForeColor = Settings.User.GetColor("editor.regexForeColor", Color.FromArgb(0xFF00FF));
            EditorCommentLineDocForeColor = Settings.User.GetColor("editor.commentLineDocForeColor", Color.FromArgb(0x77A7DB));
            EditorWordForeColor = Settings.User.GetColor("editor.wordForeColor", Color.FromArgb(0x48A8EE));
            EditorWord2ForeColor = Settings.User.GetColor("editor.word2ForeColor", Color.Teal); //Color.FromArgb(0xF98906);
            EditorCommentDocKeyWordForeColor = Settings.User.GetColor("editor.commentDocKeywordForeColor", Color.FromArgb(0xB3D991));
            EditorCommentDocKeywordErrorForeColor = Settings.User.GetColor("editor.commentDocKeywordErrorForeColor", Color.FromArgb(0xFF0000));
            EditorGlobalClassForeColor = Settings.User.GetColor("editor.globalClassForeColor", Color.FromArgb(100, 100, 200));//Color.FromArgb(0x48A8EE);
            EditorMarkersForeColor = Settings.User.GetColor("editor.markersForeColor", EditorMarginBackColor);
            EditorMarkersBackColor = Settings.User.GetColor("editor.markersBackColor", Color.Silver);

            EditorFontBold = Settings.User.GetBool("editor.fontBold", false);
            EditorLineNumberFontBold = Settings.User.GetBool("editor.lineNumberFontBold", EditorFontBold);
            EditorIdentifierFontBold = Settings.User.GetBool("editor.identifierFontBold", EditorFontBold);
            EditorCommentFontBold = Settings.User.GetBool("editor.commentFontBold", EditorFontBold);
            EditorCommentLineFontBold = Settings.User.GetBool("editor.commentLineFontBold", EditorFontBold);
            EditorCommentDocFontBold = Settings.User.GetBool("editor.commentDocFontBold", EditorFontBold);
            EditorNumberFontBold = Settings.User.GetBool("editor.numberFontBold", EditorFontBold);
            EditorStringFontBold = Settings.User.GetBool("editor.stringFontBold", EditorFontBold);
            EditorCharacterFontBold = Settings.User.GetBool("editor.characterFontBold", EditorFontBold);
            EditorPreprocessorFontBold = Settings.User.GetBool("editor.preprocessorFontBold", EditorFontBold);
            EditorOperatorFontBold = Settings.User.GetBool("editor.operatorFontBold", EditorFontBold);
            EditorRegexFontBold = Settings.User.GetBool("editor.regexFontBold", EditorFontBold);
            EditorCommentLineDocFontBold = Settings.User.GetBool("editor.commentLineDocFontBold", EditorFontBold);
            EditorWordFontBold = Settings.User.GetBool("editor.wordFontBold", EditorFontBold);
            EditorWord2FontBold = Settings.User.GetBool("editor.word2FontBold", EditorFontBold);
            EditorCommentDocKeyWordFontBold = Settings.User.GetBool("editor.commentDocKeywordFontBold", EditorFontBold);
            EditorCommentDocKeywordErrorFontBold = Settings.User.GetBool("editor.commentDocKeywordErrorFontBold", EditorFontBold);
            EditorGlobalClassFontBold = Settings.User.GetBool("editor.globalClassFontBold", EditorFontBold);
            EditorMarkersFontBold = Settings.User.GetBool("editor.markersFontBold", EditorFontBold);
        }
    }
}
