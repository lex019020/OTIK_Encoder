# OTIK_Encoder

### File header signature:

    4 bytes: 4b 45 4b 57 (ASCII: KEKW)
    1 byte: version (v1 = 0)
    1 byte: random splitting (0 - none, X - on)
    1 byte: entropic compression type (0 - none)
    1 byte: context-based compression type (0 - none)
    1 byte: anti-interference type (0 - none)
    3 bytes: files count 

    for each file:
        2 bytes: filename size (in bytes, M)
        M bytes: filename
        4 bytes: file size (in bytes, N)
        file data:

        if random splitting = on

            4 bytes: blocks count
            for each block:
                1 byte: block size (bytes, K)
                K bytes: data
        else
            N bytes: raw data
        endif
