#!/bin/sh
#
# Copyright (c) 2006 - 2007 Kungliga Tekniska H�gskolan
# (Royal Institute of Technology, Stockholm, Sweden). 
# All rights reserved. 
#
# Redistribution and use in source and binary forms, with or without 
# modification, are permitted provided that the following conditions 
# are met: 
#
# 1. Redistributions of source code must retain the above copyright 
#    notice, this list of conditions and the following disclaimer. 
#
# 2. Redistributions in binary form must reproduce the above copyright 
#    notice, this list of conditions and the following disclaimer in the 
#    documentation and/or other materials provided with the distribution. 
#
# 3. Neither the name of the Institute nor the names of its contributors 
#    may be used to endorse or promote products derived from this software 
#    without specific prior written permission. 
#
# THIS SOFTWARE IS PROVIDED BY THE INSTITUTE AND CONTRIBUTORS ``AS IS'' AND 
# ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
# IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
# ARE DISCLAIMED.  IN NO EVENT SHALL THE INSTITUTE OR CONTRIBUTORS BE LIABLE 
# FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
# DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS 
# OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
# HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
# LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY 
# OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
# SUCH DAMAGE. 


while true
do
  case $1 in
  -s) tsrcdir="$2"; shift 2;;
  -o) tobjdir="$2"; shift 2;;
  -*) echo "$0: Bad option $1"; echo $usage; exit 1;;
  *) break;;
  esac
done

if [ X"${tobjdir}" = "X" -o X"${tsrcdir}" = X ] ; then
    echo "tobjdir or tsrcdir not defined"
    exit 1
fi

if [ ! -f "${tobjdir}/libtool" ]; then
    echo "libtool missing from \"${tobjdir}\""
    exit 1
fi

doit=1

libtool="${tobjdir}/libtool --mode=execute"

valgrind="valgrind --leak-check=full --trace-children=yes --quiet -q --num-callers=30 --suppressions=${tsrcdir}/cf/valgrind-suppressions"

if head -10 "$1" | grep 'Generated by ltmain.sh' > /dev/null ; then
    uselibtool=1
elif head -10 "$1" | grep 'bin/sh' > /dev/null ; then
    libtool=
    valgrind=
fi

exec $libtool $valgrind "$@"
