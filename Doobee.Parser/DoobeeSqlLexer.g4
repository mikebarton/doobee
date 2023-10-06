lexer grammar DoobeeSqlLexer;

options { caseInsensitive = true; }



DROP: D R O P;
CREATE: C R E A T E;
TABLE: T A B L E;
INSERT: I N S E R T;
INTO: I N T O;
VALUES: V A L U E S;
NOT: N O T;
NULL: N U L L;
PRIMARY: P R I M A R Y;
KEY: K E Y;
TRUE: T R U E;
FALSE: F A L S E;

INT: I N T;
TEXT: T E X T;
BOOL: B O O L;

ID: [A-Za-z_#]([A-Za-z#_0-9])*;
STRING_LITERAL: '\'' ( ~'\'' | '\'\'')* '\'';
NUMERIC_LITERAL: ((DIGIT+ ('.' DIGIT*)?) | ('.' DIGIT+)) ('E' [-+]? DIGIT+)? | '0x' HEX_DIGIT+;


LEFT_BRACKET: '(';
RIGHT_BRACKET: ')';
COMMA: ',';


fragment A: [Aa];
fragment B: [Bb];
fragment C: [Cc];
fragment D: [Dd];
fragment E: [Ee];
fragment F: [Ff];
fragment G: [Gg];
fragment H: [Hh];
fragment I: [Ii];
fragment J: [Jj];
fragment K: [Kk];
fragment L: [Ll];
fragment M: [Mm];
fragment N: [Nn];
fragment O: [Oo];
fragment P: [Pp];
fragment Q: [Qq];
fragment R: [Rr];
fragment S: [Ss];
fragment T: [Tt];
fragment U: [Uu];
fragment V: [Vv];
fragment W: [Ww];
fragment X: [Xx];
fragment Y: [Yy];
fragment Z: [Zz];

fragment HEX_DIGIT: [0-9A-F];
fragment DIGIT:     [0-9];