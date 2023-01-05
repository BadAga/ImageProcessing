.data 
 valMulti real4 4 dup (255.0)
 valOne real4 4 dup (1.0)
.code
    MyProc proc
                ; rcx - correction array(look up table)
                ; rdx - original array
                ; r8 - result array
                ; r9 - pic width
                mov r12,0; initialize counter within row
                mov r11,0

    ProcessRow: cmp r12, r9
                je Finish                

                mov r13,r12             ;get adress of rdx
                add r13,rdx             ;add counter to rdx adress
                
                mov eax,dword ptr [r13] ;get value at said adress index= of correction table
                shl eax,2
                                        ;get adress of correction table cell at said index
                mov dword ptr[r13],eax
                
                

                add r11,1
                cmp r11,4
                je Alfa

                movss xmm1, dword ptr[rcx+rax]
                mulss xmm1, dword ptr[valMulti]
                movss dword ptr[r8+r12],xmm1
                add r12, 4
                jmp ProcessRow

        Alfa:   mov r11,0
                movss xmm1, dword ptr[valOne]
                mulss xmm1, dword ptr[valMulti]
                movss dword ptr[r8+r12],xmm1

                add r12, 4
                jmp ProcessRow


        Finish: ret
                MyProc endp
                end
