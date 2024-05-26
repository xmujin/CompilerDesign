%namespace myapp.Model.Lexer
%scannertype LexerScanner
%visibility public

%option noparser, noembedbuffer

%x COMMENT

%{

%}

Number          ([1-9][0-9]*)|(0[0-7]*)|(0x[0-9a-fA-F]+)
Identifier		[a-zA-Y_][a-zA-Z0-9_]*
Operator		[*+-/=<>%]
DoubleOpt		([!*+-/<=>]=)|&&|\|\||\+\+
Other           [^; \n\/\r\t]
Float           [-+]?[0-9]+\.[0-9]+([eE][-+]?[0-9]+)?
Char            '.'
String          \".*\"
Boundary		[;{}(),]
%%

"/*"			{BEGIN(COMMENT);}
<COMMENT>.		{}
<COMMENT>"*/"	{BEGIN(INITIAL);}
"//".*			{}



{Number}		{ ShowNum(); }
{Identifier}	{ ShowId();}
{DoubleOpt}		{ ShowDoubleOpet();}
{Operator}      { ShowOperator();}
{Float}         { ShowNum();}
{Char}          { ShowNum();}
{String}        { ShowNum();}
{Boundary}		{ ShowBoundary();}
{Number}{Identifier}        {ReportError();}

%%

