@ECHO OFF
FOR %%F IN (pretex/*.*) DO (
pandoc -f markdown -t latex -o tex/%%~nF.tex pretex/%%F
ECHO pandoc -f markdown -t latex -o tex/%%~nF.tex pretex/%%F
)

pdflatex -interaction=batchmode -synctex=1 tex/rb3007130239.tex output-directory tex/pdf/
pdflatex -interaction=batchmode -synctex=1 tex/rb3007130239.tex output-directory tex/pdf/