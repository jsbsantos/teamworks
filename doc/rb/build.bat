﻿echo off
pandoc --from=markdown --to=latex --output=D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\aplicacao-web.tex D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\pretex\aplicacao-web.pretex
pandoc --from=markdown --to=latex --output=D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\auth.tex D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\pretex\auth.pretex
pandoc --from=markdown --to=latex --output=D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\conclusao.tex D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\pretex\conclusao.pretex
pandoc --from=markdown --to=latex --output=D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\ddd.tex D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\pretex\ddd.pretex
pandoc --from=markdown --to=latex --output=D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\descricao-geral.tex D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\pretex\descricao-geral.pretex
pandoc --from=markdown --to=latex --output=D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\domain.tex D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\pretex\domain.pretex
pandoc --from=markdown --to=latex --output=D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\glossario.tex D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\pretex\glossario.pretex
pandoc --from=markdown --to=latex --output=D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\introducao.tex D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\pretex\introducao.pretex
pandoc --from=markdown --to=latex --output=D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\model.tex D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\pretex\model.pretex
pandoc --from=markdown --to=latex --output=D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\web-api.tex D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\pretex\web-api.pretex
copy NUL blank.md
pandoc --variable lang=portuguese --variable linkcolor=black --variable tables=true --variable graphics=true --from=markdown --to=latex --output=D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\rb3007130239.tex --listings --include-in-header=D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\header.tex --standalone --template=D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\template.latex  --number-sections --include-before-body=D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\front.tex --toc  --include-after-body="D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\introducao.tex" --include-after-body="D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\descricao-geral.tex" --include-after-body="D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\web-api.tex" --include-after-body="D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\aplicacao-web.tex" --include-after-body="D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\conclusao.tex" --include-after-body="D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\glossario.tex" .\blank.md
del blank.md
pdflatex -output-directory D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\pdf -interaction=nonstopmode -synctex=1 D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\cover.tex
pdflatex -output-directory D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\pdf -interaction=nonstopmode -synctex=1 D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\rb3007130239.tex
pdflatex -output-directory D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\pdf -interaction=nonstopmode -synctex=1 D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\output\rb3007130239.tex
copy D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\pdf\rb3007130239.pdf D:\Documents\Dropbox\isel\ps1112v\li61n-g07\doc\rb\rb3007130239.pdf
