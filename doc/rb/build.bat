@ECHO OFF
FOR %%F IN (pretex/*.*) DO (
pandoc -f markdown -t latex -o tex/%%~nF.tex pretex/%%F
ECHO pandoc -f markdown -t latex -o tex/%%~nF.tex pretex/%%F
)

ECHO y | DEL tex\pdf\*.*
ECHO y | DEL tex\pdf\tex\*.*

pdflatex -output-directory ./tex/pdf/ -interaction=batchmode -synctex=1 tex/rb3007130239.tex 
pdflatex -output-directory ./tex/pdf/ -interaction=batchmode -synctex=1 tex/rb3007130239.tex 