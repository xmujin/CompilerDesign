//
//  This CSharp output file generated by Gardens Point LEX
//  Gardens Point LEX (GPLEX) is Copyright (c) John Gough, QUT 2006-2014.
//  Output produced by GPLEX is the property of the user.
//  See accompanying file GPLEXcopyright.rtf.
//
//  GPLEX Version:  1.2.2
//  DateTime: 2024/5/26 18:56:29
//  GPLEX input file <Model\Lexer\MyAutoLexer.lex - 2024/5/26 18:56:26>
//  GPLEX frame file <embedded resource>
//
//  Option settings: noParser, minimize
//  Option settings: compressNext, persistBuffer, noEmbedBuffers
//

//
// Revised backup code
// Version 1.2.1 of 24-June-2013
//
//
#define BACKUP
#define STANDALONE
#define PERSIST
#define BYTEMODE

using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;

using QUT.GplexBuffers;

namespace myapp.Model.Lexer
{   
    /// <summary>
    /// Summary Canonical example of GPLEX automaton
    /// </summary>
    
#if STANDALONE
    //
    // These are the dummy declarations for stand-alone GPLEX applications
    // normally these declarations would come from the parser.
    // If you declare /noparser, or %option noparser then you get this.
    //

     public enum Tokens
    { 
      EOF = 0, maxParseToken = int.MaxValue 
      // must have at least these two, values are almost arbitrary
    }

     public abstract class ScanBase
    {
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "yylex")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "yylex")]
        public abstract int yylex();

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "yywrap")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "yywrap")]
        protected virtual bool yywrap() { return true; }

#if BABEL
        protected abstract int CurrentSc { get; set; }
        // EolState is the 32-bit of state data persisted at 
        // the end of each line for Visual Studio colorization.  
        // The default is to return CurrentSc.  You must override
        // this if you want more complicated behavior.
        public virtual int EolState { 
            get { return CurrentSc; }
            set { CurrentSc = value; } 
        }
    }
    
     public interface IColorScan
    {
        void SetSource(string source, int offset);
        int GetNext(ref int state, out int start, out int end);
#endif // BABEL
    }

#endif // STANDALONE
    
    // If the compiler can't find the scanner base class maybe you
    // need to run GPPG with the /gplex option, or GPLEX with /noparser
#if BABEL
     public sealed partial class LexerScanner : ScanBase, IColorScan
    {
        private ScanBuff buffer;
        int currentScOrd;  // start condition ordinal
        
        protected override int CurrentSc 
        {
             // The current start state is a property
             // to try to avoid the user error of setting
             // scState but forgetting to update the FSA
             // start state "currentStart"
             //
             get { return currentScOrd; }  // i.e. return YY_START;
             set { currentScOrd = value;   // i.e. BEGIN(value);
                   currentStart = startState[value]; }
        }
#else  // BABEL
     public sealed partial class LexerScanner : ScanBase
    {
        private ScanBuff buffer;
        int currentScOrd;  // start condition ordinal
#endif // BABEL
        
        /// <summary>
        /// The input buffer for this scanner.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public ScanBuff Buffer { get { return buffer; } }
        
        private static int GetMaxParseToken() {
     System.Reflection.FieldInfo f = typeof(Tokens).GetField("maxParseToken");
            return (f == null ? int.MaxValue : (int)f.GetValue(null));
        }
        
        static int parserMax = GetMaxParseToken();
        
        enum Result {accept, noMatch, contextFound};

        const int maxAccept = 23;
        const int initial = 24;
        const int eofNum = 0;
        const int goStart = -1;
        const int INITIAL = 0;
        const int COMMENT = 1;

#region user code
#endregion user code

        int state;
        int currentStart = startState[0];
        int code;      // last code read
        int cCol;      // column number of code
        int lNum;      // current line number
        //
        // The following instance variables are used, among other
        // things, for constructing the yylloc location objects.
        //
        int tokPos;        // buffer position at start of token
        int tokCol;        // zero-based column number at start of token
        int tokLin;        // line number at start of token
        int tokEPos;       // buffer position at end of token
        int tokECol;       // column number at end of token
        int tokELin;       // line number at end of token
        string tokTxt;     // lazily constructed text of token
#if STACK          
        private Stack<int> scStack = new Stack<int>();
#endif // STACK

#region ScannerTables
    struct Table {
        public int min; public int rng; public int dflt;
        public sbyte[] nxt;
        public Table(int m, int x, int d, sbyte[] n) {
            min = m; rng = x; dflt = d; nxt = n;
        }
    };

    static int[] startState = new int[] {24, 35, 0};

    static Table[] NxS = new Table[36] {
/* NxS[   0] */ new Table(0, 0, 0, null),
/* NxS[   1] */ new Table(0, 0, -1, null),
/* NxS[   2] */ new Table(0, 0, -1, null),
/* NxS[   3] */ new Table(61, 1, -1, new sbyte[] {10}),
/* NxS[   4] */ new Table(43, 19, -1, new sbyte[] {10, -1, -1, -1, -1, 33, 
          33, 33, 33, 33, 33, 33, 33, 33, 33, -1, -1, -1, 10}),
/* NxS[   5] */ new Table(48, 14, -1, new sbyte[] {33, 33, 33, 33, 33, 33, 
          33, 33, 33, 33, -1, -1, -1, 10}),
/* NxS[   6] */ new Table(42, 20, -1, new sbyte[] {17, -1, -1, -1, -1, 18, 
          -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 10}),
/* NxS[   7] */ new Table(46, 77, -1, new sbyte[] {30, -1, 14, 14, 14, 14, 
          14, 14, 14, 14, 33, 33, -1, -1, -1, -1, -1, -1, -1, 11, 11, 11, 
          11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 
          11, 11, 11, 11, 11, 11, -1, -1, -1, -1, -1, 11, -1, 11, 11, 11, 
          11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 
          11, 11, 11, 11, 15, 11, 11}),
/* NxS[   8] */ new Table(46, 77, -1, new sbyte[] {30, -1, 8, 8, 8, 8, 
          8, 8, 8, 8, 8, 8, -1, -1, -1, -1, -1, -1, -1, 11, 11, 11, 
          11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 
          11, 11, 11, 11, 11, 11, -1, -1, -1, -1, -1, 11, -1, 11, 11, 11, 
          11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 
          11, 11, 11, 11, 11, 11, 11}),
/* NxS[   9] */ new Table(48, 75, -1, new sbyte[] {9, 9, 9, 9, 9, 9, 
          9, 9, 9, 9, -1, -1, -1, -1, -1, -1, -1, 9, 9, 9, 9, 9, 
          9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 
          9, 9, 9, 9, 9, -1, -1, -1, -1, 9, -1, 9, 9, 9, 9, 9, 
          9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 
          9, 9, 9, 9, 9}),
/* NxS[  10] */ new Table(0, 0, -1, null),
/* NxS[  11] */ new Table(48, 75, -1, new sbyte[] {11, 11, 11, 11, 11, 11, 
          11, 11, 11, 11, -1, -1, -1, -1, -1, -1, -1, 11, 11, 11, 11, 11, 
          11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 
          11, 11, 11, 11, 11, -1, -1, -1, -1, 11, -1, 11, 11, 11, 11, 11, 
          11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 
          11, 11, 11, 11, 11}),
/* NxS[  12] */ new Table(48, 54, -1, new sbyte[] {12, 12, 12, 12, 12, 12, 
          12, 12, 12, 12, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 31, 
          -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 
          -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 31}),
/* NxS[  13] */ new Table(48, 10, -1, new sbyte[] {13, 13, 13, 13, 13, 13, 
          13, 13, 13, 13}),
/* NxS[  14] */ new Table(46, 77, -1, new sbyte[] {30, -1, 14, 14, 14, 14, 
          14, 14, 14, 14, 33, 33, -1, -1, -1, -1, -1, -1, -1, 11, 11, 11, 
          11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 
          11, 11, 11, 11, 11, 11, -1, -1, -1, -1, -1, 11, -1, 11, 11, 11, 
          11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 
          11, 11, 11, 11, 11, 11, 11}),
/* NxS[  15] */ new Table(48, 75, -1, new sbyte[] {16, 16, 16, 16, 16, 16, 
          16, 16, 16, 16, -1, -1, -1, -1, -1, -1, -1, 16, 16, 16, 16, 16, 
          16, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 
          11, 11, 11, 11, 11, -1, -1, -1, -1, 11, -1, 16, 16, 16, 16, 16, 
          16, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 
          11, 11, 11, 11, 11}),
/* NxS[  16] */ new Table(48, 75, -1, new sbyte[] {16, 16, 16, 16, 16, 16, 
          16, 16, 16, 16, -1, -1, -1, -1, -1, -1, -1, 16, 16, 16, 16, 16, 
          16, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 
          11, 11, 11, 11, 11, -1, -1, -1, -1, 11, -1, 16, 16, 16, 16, 16, 
          16, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 
          11, 11, 11, 11, 11}),
/* NxS[  17] */ new Table(0, 0, -1, null),
/* NxS[  18] */ new Table(10, 1, 18, new sbyte[] {-1}),
/* NxS[  19] */ new Table(0, 0, -1, null),
/* NxS[  20] */ new Table(10, 25, 26, new sbyte[] {-1, 26, 26, 26, 26, 26, 
          26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 
          26, 26, 20}),
/* NxS[  21] */ new Table(0, 0, -1, null),
/* NxS[  22] */ new Table(47, 1, -1, new sbyte[] {23}),
/* NxS[  23] */ new Table(0, 0, -1, null),
/* NxS[  24] */ new Table(33, 93, -1, new sbyte[] {25, 26, -1, -1, 1, 27, 
          28, 2, 2, 3, 4, 3, 5, 3, 6, 7, 8, 8, 8, 8, 8, 8, 
          8, 8, 8, -1, 2, 3, 3, 3, -1, -1, 9, 9, 9, 9, 9, 9, 
          9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 
          9, 9, 9, -1, -1, -1, -1, -1, 9, -1, 9, 9, 9, 9, 9, 9, 
          9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 
          9, 9, 9, 9, 2, 29, 2}),
/* NxS[  25] */ new Table(61, 1, -1, new sbyte[] {10}),
/* NxS[  26] */ new Table(10, 25, 26, new sbyte[] {-1, 26, 26, 26, 26, 26, 
          26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 
          26, 26, 20}),
/* NxS[  27] */ new Table(38, 1, -1, new sbyte[] {10}),
/* NxS[  28] */ new Table(10, 1, 34, new sbyte[] {-1}),
/* NxS[  29] */ new Table(124, 1, -1, new sbyte[] {10}),
/* NxS[  30] */ new Table(48, 10, -1, new sbyte[] {12, 12, 12, 12, 12, 12, 
          12, 12, 12, 12}),
/* NxS[  31] */ new Table(43, 15, -1, new sbyte[] {32, -1, 32, -1, -1, 13, 
          13, 13, 13, 13, 13, 13, 13, 13, 13}),
/* NxS[  32] */ new Table(48, 10, -1, new sbyte[] {13, 13, 13, 13, 13, 13, 
          13, 13, 13, 13}),
/* NxS[  33] */ new Table(46, 12, -1, new sbyte[] {30, -1, 33, 33, 33, 33, 
          33, 33, 33, 33, 33, 33}),
/* NxS[  34] */ new Table(39, 1, -1, new sbyte[] {19}),
/* NxS[  35] */ new Table(10, 33, 21, new sbyte[] {-1, 21, 21, 21, 21, 21, 
          21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 
          21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 22}),
    };

int NextState() {
    if (code == ScanBuff.EndOfFile)
        return eofNum;
    else
        unchecked {
            int rslt;
            int idx = (byte)(code - NxS[state].min);
            if ((uint)idx >= (uint)NxS[state].rng) rslt = NxS[state].dflt;
            else rslt = NxS[state].nxt[idx];
            return rslt;
        }
}

#endregion


#if BACKUP
        // ==============================================================
        // == Nested struct used for backup in automata that do backup ==
        // ==============================================================

        struct Context // class used for automaton backup.
        {
            public int bPos;
            public int rPos; // scanner.readPos saved value
            public int cCol;
            public int lNum; // Need this in case of backup over EOL.
            public int state;
            public int cChr;
        }
        
        private Context ctx = new Context();
#endif // BACKUP

        // ==============================================================
        // ==== Nested struct to support input switching in scanners ====
        // ==============================================================

		struct BufferContext {
            internal ScanBuff buffSv;
			internal int chrSv;
			internal int cColSv;
			internal int lNumSv;
		}

        // ==============================================================
        // ===== Private methods to save and restore buffer contexts ====
        // ==============================================================

        /// <summary>
        /// This method creates a buffer context record from
        /// the current buffer object, together with some
        /// scanner state values. 
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        BufferContext MkBuffCtx()
		{
			BufferContext rslt;
			rslt.buffSv = this.buffer;
			rslt.chrSv = this.code;
			rslt.cColSv = this.cCol;
			rslt.lNumSv = this.lNum;
			return rslt;
		}

        /// <summary>
        /// This method restores the buffer value and allied
        /// scanner state from the given context record value.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        void RestoreBuffCtx(BufferContext value)
		{
			this.buffer = value.buffSv;
			this.code = value.chrSv;
			this.cCol = value.cColSv;
			this.lNum = value.lNumSv;
        } 
        // =================== End Nested classes =======================

#if !NOFILES
     public LexerScanner(Stream file) {
            SetSource(file); // no unicode option
        }   
#endif // !NOFILES

     public LexerScanner() { }

        private int readPos;

        void GetCode()
        {
            if (code == '\n')  // This needs to be fixed for other conventions
                               // i.e. [\r\n\205\u2028\u2029]
            { 
                cCol = -1;
                lNum++;
            }
            readPos = buffer.Pos;

            // Now read new codepoint.
            code = buffer.Read();
            if (code > ScanBuff.EndOfFile)
            {
#if (!BYTEMODE)
                if (code >= 0xD800 && code <= 0xDBFF)
                {
                    int next = buffer.Read();
                    if (next < 0xDC00 || next > 0xDFFF)
                        code = ScanBuff.UnicodeReplacementChar;
                    else
                        code = (0x10000 + ((code & 0x3FF) << 10) + (next & 0x3FF));
                }
#endif
                cCol++;
            }
        }

        void MarkToken()
        {
#if (!PERSIST)
            buffer.Mark();
#endif
            tokPos = readPos;
            tokLin = lNum;
            tokCol = cCol;
        }
        
        void MarkEnd()
        {
            tokTxt = null;
            tokEPos = readPos;
            tokELin = lNum;
            tokECol = cCol;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        int Peek()
        {
            int rslt, codeSv = code, cColSv = cCol, lNumSv = lNum, bPosSv = buffer.Pos;
            GetCode(); rslt = code;
            lNum = lNumSv; cCol = cColSv; code = codeSv; buffer.Pos = bPosSv;
            return rslt;
        }

        // ==============================================================
        // =====    Initialization of string-based input buffers     ====
        // ==============================================================

        /// <summary>
        /// Create and initialize a StringBuff buffer object for this scanner
        /// </summary>
        /// <param name="source">the input string</param>
        /// <param name="offset">starting offset in the string</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public void SetSource(string source, int offset)
        {
            this.buffer = ScanBuff.GetBuffer(source);
            this.buffer.Pos = offset;
            this.lNum = 0;
            this.code = '\n'; // to initialize yyline, yycol and lineStart
            GetCode();
        }

        // ================ LineBuffer Initialization ===================
        /// <summary>
        /// Create and initialize a LineBuff buffer object for this scanner
        /// </summary>
        /// <param name="source">the list of input strings</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public void SetSource(IList<string> source)
        {
            this.buffer = ScanBuff.GetBuffer(source);
            this.code = '\n'; // to initialize yyline, yycol and lineStart
            this.lNum = 0;
            GetCode();
        }

#if !NOFILES        
        // =============== StreamBuffer Initialization ==================

        /// <summary>
        /// Create and initialize a StreamBuff buffer object for this scanner.
        /// StreamBuff is buffer for 8-bit byte files.
        /// </summary>
        /// <param name="source">the input byte stream</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public void SetSource(Stream source)
        {
            this.buffer = ScanBuff.GetBuffer(source);
            this.lNum = 0;
            this.code = '\n'; // to initialize yyline, yycol and lineStart
            GetCode();
        }
        
#if !BYTEMODE
        // ================ TextBuffer Initialization ===================

        /// <summary>
        /// Create and initialize a TextBuff buffer object for this scanner.
        /// TextBuff is a buffer for encoded unicode files.
        /// </summary>
        /// <param name="source">the input text file</param>
        /// <param name="fallbackCodePage">Code page to use if file has
        /// no BOM. For 0, use machine default; for -1, 8-bit binary</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public void SetSource(Stream source, int fallbackCodePage)
        {
            this.buffer = ScanBuff.GetBuffer(source, fallbackCodePage);
            this.lNum = 0;
            this.code = '\n'; // to initialize yyline, yycol and lineStart
            GetCode();
        }
#endif // !BYTEMODE
#endif // !NOFILES
        
        // ==============================================================

#if BABEL
        //
        //  Get the next token for Visual Studio
        //
        //  "state" is the inout mode variable that maintains scanner
        //  state between calls, using the EolState property. In principle,
        //  if the calls of EolState are costly set could be called once
        //  only per line, at the start; and get called only at the end
        //  of the line. This needs more infrastructure ...
        //
        public int GetNext(ref int state, out int start, out int end)
        {
                Tokens next;
            int s, e;
            s = state;        // state at start
            EolState = state;
                next = (Tokens)Scan();
            state = EolState;
            e = state;       // state at end;
            start = tokPos;
            end = tokEPos - 1; // end is the index of last char.
            return (int)next;
        }        
#endif // BABEL

        // ======== AbstractScanner<> Implementation =========

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "yylex")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "yylex")]
        public override int yylex()
        {
            // parserMax is set by reflecting on the Tokens
            // enumeration.  If maxParseToken is defined
            // that is used, otherwise int.MaxValue is used.
            int next;
            do { next = Scan(); } while (next >= parserMax);
            return next;
        }
        
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        int yypos { get { return tokPos; } }
        
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        int yyline { get { return tokLin; } }
        
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        int yycol { get { return tokCol; } }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "yytext")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "yytext")]
        public string yytext
        {
            get 
            {
                if (tokTxt == null) 
                    tokTxt = buffer.GetString(tokPos, tokEPos);
                return tokTxt;
            }
        }

        /// <summary>
        /// Discards all but the first "n" codepoints in the recognized pattern.
        /// Resets the buffer position so that only n codepoints have been consumed;
        /// yytext is also re-evaluated. 
        /// </summary>
        /// <param name="n">The number of codepoints to consume</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        void yyless(int n)
        {
            buffer.Pos = tokPos;
            // Must read at least one char, so set before start.
            cCol = tokCol - 1; 
            GetCode();
            // Now ensure that line counting is correct.
            lNum = tokLin;
            // And count the rest of the text.
            for (int i = 0; i < n; i++) GetCode();
            MarkEnd();
        }
       
        //
        //  It would be nice to count backward in the text
        //  but it does not seem possible to re-establish
        //  the correct column counts except by going forward.
        //
        /// <summary>
        /// Removes the last "n" code points from the pattern.
        /// </summary>
        /// <param name="n">The number to remove</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        void _yytrunc(int n) { yyless(yyleng - n); }
        
        //
        // This is painful, but we no longer count
        // codepoints.  For the overwhelming majority 
        // of cases the single line code is fast, for
        // the others, well, at least it is all in the
        // buffer so no files are touched. Note that we
        // can't use (tokEPos - tokPos) because of the
        // possibility of surrogate pairs in the token.
        //
        /// <summary>
        /// The length of the pattern in codepoints (not the same as 
        /// string-length if the pattern contains any surrogate pairs).
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "yyleng")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "yyleng")]
        public int yyleng
        {
            get {
                if (tokELin == tokLin)
                    return tokECol - tokCol;
                else
#if BYTEMODE
                    return tokEPos - tokPos;
#else
                {
                    int ch;
                    int count = 0;
                    int save = buffer.Pos;
                    buffer.Pos = tokPos;
                    do {
                        ch = buffer.Read();
                        if (!char.IsHighSurrogate((char)ch)) count++;
                    } while (buffer.Pos < tokEPos && ch != ScanBuff.EndOfFile);
                    buffer.Pos = save;
                    return count;
                }
#endif // BYTEMODE
            }
        }
        
        // ============ methods available in actions ==============

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal int YY_START {
            get { return currentScOrd; }
            set { currentScOrd = value; 
                  currentStart = startState[value]; 
            } 
        }
        
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal void BEGIN(int next) {
            currentScOrd = next;
            currentStart = startState[next];
        }

        // ============== The main tokenizer code =================

        int Scan() {
                for (; ; ) {
                    int next;              // next state to enter
#if LEFTANCHORS
                    for (;;) {
                        // Discard characters that do not start any pattern.
                        // Must check the left anchor condition after *every* GetCode!
                        state = ((cCol == 0) ? anchorState[currentScOrd] : currentStart);
                        if ((next = NextState()) != goStart) break; // LOOP EXIT HERE...
                        GetCode();
                    }
                    
#else // !LEFTANCHORS
                    state = currentStart;
                    while ((next = NextState()) == goStart) {
                        // At this point, the current character has no
                        // transition from the current state.  We discard 
                        // the "no-match" char.   In traditional LEX such 
                        // characters are echoed to the console.
                        GetCode();
                    }
#endif // LEFTANCHORS                    
                    // At last, a valid transition ...    
                    MarkToken();
                    state = next;
                    GetCode();                    
#if BACKUP
                    bool contextSaved = false;
                    while ((next = NextState()) > eofNum) { // Exit for goStart AND for eofNum
                        if (state <= maxAccept && next > maxAccept) { // need to prepare backup data
                            // Store data for the *latest* accept state that was found.
                            SaveStateAndPos( ref ctx );
                            contextSaved = true;
                        }
                        state = next;
                        GetCode();
                    }
                    if (state > maxAccept && contextSaved)
                        RestoreStateAndPos( ref ctx );
#else  // BACKUP
                    while ((next = NextState()) > eofNum) { // Exit for goStart AND for eofNum
                         state = next;
                         GetCode();
                    }
#endif // BACKUP
                    if (state <= maxAccept) {
                        MarkEnd();
#region ActionSwitch
#pragma warning disable 162, 1522
    switch (state)
    {
        case eofNum:
            if (yywrap())
                return (int)Tokens.EOF;
            break;
        case 1:
        case 3:
        case 4:
        case 5:
        case 6:
ShowOperator();
            break;
        case 2:
ShowBoundary();
            break;
        case 7:
        case 8:
        case 14:
        case 16:
ShowNum();
            break;
        case 9:
ShowId();
            break;
        case 10:
ShowDoubleOpet();
            break;
        case 11:
        case 15:
ReportError();
            break;
        case 12:
        case 13:
ShowNum();
            break;
        case 17:
BEGIN(COMMENT);
            break;
        case 18:
{}
            break;
        case 19:
ShowNum();
            break;
        case 20:
ShowNum();
            break;
        case 21:
        case 22:
{}
            break;
        case 23:
BEGIN(INITIAL);
            break;
        default:
            break;
    }
#pragma warning restore 162, 1522
#endregion
                    }
                }
        }

#if BACKUP
        void SaveStateAndPos(ref Context ctx) {
            ctx.bPos  = buffer.Pos;
            ctx.rPos  = readPos;
            ctx.cCol  = cCol;
            ctx.lNum  = lNum;
            ctx.state = state;
            ctx.cChr  = code;
        }

        void RestoreStateAndPos(ref Context ctx) {
            buffer.Pos = ctx.bPos;
            readPos = ctx.rPos;
            cCol  = ctx.cCol;
            lNum  = ctx.lNum;
            state = ctx.state;
            code  = ctx.cChr;
        }
#endif  // BACKUP

        // ============= End of the tokenizer code ================

#if STACK        
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal void yy_clear_stack() { scStack.Clear(); }
        
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal int yy_top_state() { return scStack.Peek(); }
        
        internal void yy_push_state(int state)
        {
            scStack.Push(currentScOrd);
            BEGIN(state);
        }
        
        internal void yy_pop_state()
        {
            // Protect against input errors that pop too far ...
            if (scStack.Count > 0) {
				int newSc = scStack.Pop();
				BEGIN(newSc);
            } // Otherwise leave stack unchanged.
        }
 #endif // STACK

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal void ECHO() { Console.Out.Write(yytext); }
        
    } // end class $Scanner


} // end namespace
