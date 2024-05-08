//#define AUTO

namespace myapp.Model.Lexer
{


    public class Tag
    {
        public const int
   /*         ID      = 706, IF = 707, ELSE   = 708,
            WHILE   = 709, DO = 710, BREAK  = 711,
            AND     = 712, OR = 713, EQ     = 714,
            NE      = 715, LT = 716, GT     = 717,
            TRUE    = 718,
            FALSE   = 719,
            BASIC   = 720, // 基础类型的种别码
            CMT     = 721, // 注释符号
            BCMT    = 722, // 块注释起始符号
            BCMTE   = 723, // 块注释结束符号
            NUM     = 724,
            REAL    = 725,
            STRING  = 726,
            CHAR    = 727,
            INC     = 728,
            INCEQ   = 729,
            ERROR   =  -1;*/
        // 常用的标点符号的种别码就为其ascii码值

            ID      = 706, IF = 707, ELSE   = 708,
            WHILE   = 709, DO = 710, BREAK  = 711,
            AND     = 712, OR = 713, EQ     = 714,
            NE      = 715, LT = 716, GT     = 717, // GT,Lt  >=,<=, EQ =, NE !=
            TRUE    = 718,
            FALSE   = 719, 
            BASIC   = 720, // 基础类型的种别码
            CMT     = 721, // 注释符号
            BCMT    = 722, // 块注释起始符号
            BCMTE   = 723, // 块注释结束符号
            NUM     = 724,
            REAL    = 725,
            STRING  = 726,
            CHAR    = 727, 
            INC     = 728, // ++
            INCEQ   = 729, // +=
            SUB     = 730, // --
            SUBEQ   = 732, // -=
            DIVEQ   = 733, // /=
            MULEQ   = 734, // *=
            INDEX   = 735,
            TEMP    = 736,
            FUNC    = 737,
            FOR    = 738,
            ERROR   =  -1;
    }
}
