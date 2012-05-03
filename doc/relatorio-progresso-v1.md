1.	Introdu��o

 O presente documento constitui o relat�rio de progresso, para a unidade curricular de Projecto e Semin�rio do ano lectivo 2011/2012, referente ao projecto intitulado �teamworks�. Este projecto � realizado pelos alunos Filipe Pinheiro e Jo�o Santos e orientado pelo docente Jo�o Pedro Patriarca.
 
 Este projecto tem como objectivo a disponibiliza��o de uma infra-estrutura de apoio � realiza��o de projectos, oferecendo funcionalidades de planeamento, monitoriza��o e registo de horas e fomentando colabora��o entre membros, atrav�s de discuss�es e partilha de informa��o relacionada com o projecto.
 
 Estas funcionalidades passam pela cria��o de planos de trabalho, afecta��o de recursos a tarefas, registo do tempo despendido na realiza��o de tarefas, monitoriza��o do estado actual do projecto, �reas de discuss�o de ideias e partilha de informa��o (ficheiros) associados ao projecto, <span style=background-color="yellow">etc</span>.

2.	Descri��o Geral

 A infra-estrutura a desenvolver tem como objectivo, como foi referido anteriormente, o aux�lio � realiza��o e gest�o de projectos oferecendo de diversas funcionalidades que permitem aos membros dedicar mais tempo ao desenvolvimento efectivo das actividades do projecto e menos tempo a tarefas administrativas.

 Para a disponibiliza��o desta infra-estrutura ser� desenvolvida uma aplica��o Web e um servi�o Web RESTful que permitam efectuar todas as ac��es descritas anteriormente. Deve ser poss�vel que os utilizadores criem Projectos, Tarefas, Registo de Tempo de Trabalho, Discuss�es, <span style=background-color="yellow">Eventos e Relat�rios</span>.

 2.1. Use case

 Para ilustrar estas ac��es foram definidos os seguintes casos de utiliza��o (use case):<span style=background-color="yellow">image1 - admin</span>

 A imagem anterior mostra os casos de utiliza��o para um utilizador com permiss�es de Administrador num Projecto.<span style=background-color="yellow">image2 - user</span>

 A imagem anterior mostra os casos de utiliza��o para um utilizador sem permiss�es de Administrador num Projecto.

 2.2. Arquitetura

 Para o desenvolvimento da aplica��o Web � usada a Framework ASP.NET MVC 4.0 e para o servi�o Web RESTful ser� usada a Framework ASP.NET Web API.
 <span style=background-color="yellow">
 &lt;mais coisas&gt;
 &lt;image3 � arq&gt;
 </span>

3.	Modelo de dominio

 3.1. Aggregates, Repository, Factory (DDD)

 3.2. BD / UML / EA

4.	Web

 4.1. Site - fampinheiro

    4.1.1. Twitter Bootstrap - fampinheiro
	
    4.1.2. HTML5 + CSS3 - fampinheiro
 
 4.2. API (RESTful) - J

 4.2.1. Web Api - J

5.	Ferramentas usadas
 
 No desenvolvimento deste trabalho s�o usadas aplica��es e componentes implementadas por terceiros. De forma a facilitar a obten��o dessas componentes e a sua integra��o no ambiente de desenvolvimento foi usada a extens�o NuGet, para a aplica��o Microsoft Visual Studio 2010. Esta extens�o permite fazer a procura e <i>download</i> de componentes, da sua <i>galeria online</i>, e faz a gest�o de todas as refer�ncias usadas no projecto do Visual Studio para essas componentes.

 5.1. RavenDB
 
	Uma das aplica��es usadas � o RavenDB. O RavenDB � um sistema de base de dados de documentos implementado sobre a Framework .NET. � uma solu��o transacional, que armazena os dados no formato JSON e que para leitura e escrita de dados suporta a utiliza��o da componente Linq da Framework .NET ou de uma API RESTful disponibilizada atrav�s do protocolo HTTP.
	
	Internamente, o RavenDB usa �ndices Lucene que s�o criados automaticamente com a utiliza��o do sistema ou criados explicitamente pelo programador atrav�s de express�es Map-Reduce, de forma a otimizar os acessos aos documentos.
		
    5.1.1. NoSql

 5.2. Knockout - fampinheiro

 5.3. Outras

    5.3.1. AttributeRouting - fampinheiro

    5.3.2. AutoMapper - J

6.	Trabalho Futuro / Altera��o Planeamento


