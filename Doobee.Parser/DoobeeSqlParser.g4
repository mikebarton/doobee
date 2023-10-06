parser grammar DoobeeSqlParser;

options {
    tokenVocab = DoobeeSqlLexer;
    caseInsensitive = true;
    backtrack = true;
}

parse: create_tbl_stmt EOF
     | drop_tbl_stmt EOF
     | insert_stmt EOF
;

drop_tbl_stmt: DROP TABLE ID;

create_tbl_stmt: CREATE TABLE table_name column_defs;

insert_stmt: INSERT INTO table_name 
            LEFT_BRACKET column_list RIGHT_BRACKET
            values_clause
;

value_row:
    LEFT_BRACKET value_expr (COMMA value_expr)* RIGHT_BRACKET
;

values_clause:
    VALUES value_row (COMMA value_row)*
;

column_defs: '(' column_def (',' column_def)* ')';
column_def: ID type_name column_constraint*;
column_list:  column_name (COMMA column_name)*;
column_name:
    ID
;

column_constraint: (PRIMARY KEY)
        | (NOT? NULL)
;

type_name: INT
         | TEXT
         | BOOL
;

table_name:
    ID
;



value_expr:
    literal_value
;

literal_value:
    NUMERIC_LITERAL
    | STRING_LITERAL    
    | NULL
    | TRUE
    | FALSE
;

