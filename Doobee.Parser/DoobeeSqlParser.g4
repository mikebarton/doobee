parser grammar DoobeeSqlParser;

options {
    tokenVocab = DoobeeSqlLexer;
    caseInsensitive = true;
}


create_tbl_stmt: CREATE TABLE ID '(' column_defs ')';

column_defs: column_def (',' column_def)*;
column_def: ID type_name column_constraint*;

column_constraint: (PRIMARY KEY)
        | (NOT? NULL)
;

type_name: INT
         | TEXT
         | BOOL
;