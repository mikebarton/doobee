parser grammar DoobeeSqlParser;

options {
    tokenVocab = DoobeeSqlLexer;
    caseInsensitive = true;
    backtrack = true;
}

parse: create_tbl_stmt EOF
     | drop_tbl_stmt EOF
     | insert_stmt EOF
     | select_stmt EOF
;

drop_tbl_stmt: DROP TABLE ID;

create_tbl_stmt: CREATE TABLE table_name column_defs;

insert_stmt: INSERT INTO table_name 
            LEFT_BRACKET column_list RIGHT_BRACKET
            values_clause
;

select_stmt: 
    SELECT top_count? select_columns FROM table_name where_clause?
;

select_columns:
    column_list | STAR
;

top_count:
    TOP NUMERIC_LITERAL 
;

value_row:
    LEFT_BRACKET value_expr (COMMA value_expr)* RIGHT_BRACKET
;

values_clause:
    VALUES value_row (COMMA value_row)*
;

where_clause:
    WHERE condition
;

condition:
    simpleCondition
    | condition AND simpleCondition    
    | condition OR simpleCondition     
    ;

simpleCondition:
    equalityCondition
    | rangeCondition
    | likeCondition
    | inCondition
    | isNullCondition
    | subqueryCondition
    ;   

equalityCondition:
    column_name EQUALS literal_value;

rangeCondition:
    column_name BETWEEN literal_value AND literal_value;

likeCondition:
    column_name LIKE literal_value;

inCondition:
    column_name IN (literal_value (COMMA literal_value)*);

isNullCondition:
    column_name IS NULL
    | column_name IS NOT NULL;



subqueryCondition:
    column_name EQUALS '(' select_stmt ')'
    | column_name IN '(' select_stmt ')'
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

