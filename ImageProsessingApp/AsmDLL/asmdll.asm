.data 
 valMulti real4 4 dup (255.0)
.code
MyProc proc
            ;rcx-length of array
            ;rdx-length of array
            ;r8-length of array

            mov r11,0 ;initialize counter
            mov r12,255

ProcessRow: cmp r11,r8 
            jg Finish

            movups xmm0, xmmword ptr [rcx+r11]  ;stores correction array
            mulps xmm0,xmmword ptr [valMulti]
            movups xmmword ptr[rdx+r11],xmm0    ;saves pixel values (rgba)
            
            add r11,4
            jmp ProcessRow
Finish:
ret
MyProc endp
end
