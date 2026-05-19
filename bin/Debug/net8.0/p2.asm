PROG:        START   #0
FIRST:       STL     RETADR 
            LDA     #1 
            CLEAR   X 
            LDS     #3 
            LDT     #300
LOOP:        STA     ARRAY,X 
            ADD     #1 
            ADDR    S,X 
            COMPR   X,T 
            JLT     LOOP 
            J       @RETADR
ARRAY:       RESW    #100
RETADR:      RESW    #1
            END    FIRST
