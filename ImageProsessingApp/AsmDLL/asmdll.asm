.code
MyProc proc
        
        movups xmm0, xmmword ptr [rcx]
        movups xmmword ptr[rdx],xmm0
ret
MyProc endp
end
