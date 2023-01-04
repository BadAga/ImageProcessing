.data 
 valMulti real4 4 dup (255.0)
 valThree real4 4 dup (3.0)
.code
    MyProc proc
                ; rcx - correction array(look up table)
                ; rdx - original array
                ; r8 - result array
                ; r9 - pic width
                mov r12,0; initialize counter within row

    ProcessRow: cmp r12, r9
                je Finish
                
                mov r13,r12             ;get adress of rdx
                add r13,rdx             ;add counter to rdx adress

                mov eax,dword ptr [r13] ;get value at said adress index= of correction table
                shl eax,2
                                        ;get adress of correction table cell at said index
                mov dword ptr[r13],eax
                
               movss xmm1, dword ptr[rcx+rax]
               mov r15,r8
                add r15,r12
               movss dword ptr[r15],xmm1

                add r12, 4
                jmp ProcessRow

        Finish: ret
                MyProc endp
                end
