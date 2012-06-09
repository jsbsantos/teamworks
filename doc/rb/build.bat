@ECHO OFF
FOR %%F IN (pretex/*.*) DO (
pandoc -f markdown -t latex -o tex/%%~nF.tex pretex/%%F
ECHO pandoc -f markdown -t latex -o tex/%%~nF.tex pretex/%%F
)

pdflatex -output-directory D:/MyDocuments/GitHub/LI61N-G07/doc/rb/tex/pdf/ -interaction=batchmode -synctex=1 tex/rb3007130239.tex 
pdflatex -output-directory D:/MyDocuments/GitHub/LI61N-G07/doc/rb/tex/pdf/ -interaction=batchmode -synctex=1 tex/rb3007130239.tex 