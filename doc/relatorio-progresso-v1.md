1.	Introdução

 O presente documento constitui o relatório de progresso, para a unidade curricular de Projecto e Seminário do ano lectivo 2011/2012, referente ao projecto intitulado “teamworks”. Este projecto é realizado pelos alunos Filipe Pinheiro e João Santos e orientado pelo docente João Pedro Patriarca.
 
 Este projecto tem como objectivo a disponibilização de uma infra-estrutura de apoio à realização de projectos, oferecendo funcionalidades de planeamento, monitorização e registo de horas e fomentando colaboração entre membros, através de discussões e partilha de informação relacionada com o projecto.
 
 Estas funcionalidades passam pela criação de planos de trabalho, afectação de recursos a tarefas, registo do tempo despendido na realização de tarefas, monitorização do estado actual do projecto, áreas de discussão de ideias e partilha de informação (ficheiros) associados ao projecto, <span style=background-color="yellow">etc</span>.

2.	Descrição Geral

 A infra-estrutura a desenvolver tem como objectivo, como foi referido anteriormente, o auxílio à realização e gestão de projectos oferecendo de diversas funcionalidades que permitem aos membros dedicar mais tempo ao desenvolvimento efectivo das actividades do projecto e menos tempo a tarefas administrativas.

 Para a disponibilização desta infra-estrutura será desenvolvida uma aplicação Web e um serviço Web RESTful que permitam efectuar todas as acções descritas anteriormente. Deve ser possível que os utilizadores criem Projectos, Tarefas, Registo de Tempo de Trabalho, Discussões, <span style=background-color="yellow">Eventos e Relatórios</span>.

 2.1. Use case

 Para ilustrar estas acções foram definidos os seguintes casos de utilização (use case):<span style=background-color="yellow">image1 - admin</span>

 A imagem anterior mostra os casos de utilização para um utilizador com permissões de Administrador num Projecto.<span style=background-color="yellow">image2 - user</span>

 A imagem anterior mostra os casos de utilização para um utilizador sem permissões de Administrador num Projecto.

 2.2. Arquitetura

 Para o desenvolvimento da aplicação Web é usada a Framework ASP.NET MVC 4.0 e para o serviço Web RESTful será usada a Framework ASP.NET Web API.
 <span style=background-color="yellow">
 &lt;mais coisas&gt;
 &lt;image3 – arq&gt;
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
 
 No desenvolvimento deste trabalho são usadas aplicações e componentes implementadas por terceiros. De forma a facilitar a obtenção dessas componentes e a sua integração no ambiente de desenvolvimento foi usada a extensão NuGet, para a aplicação Microsoft Visual Studio 2010. Esta extensão permite fazer a procura e <i>download</i> de componentes, da sua <i>galeria online</i>, e faz a gestão de todas as referências usadas no projecto do Visual Studio para essas componentes.

 5.1. RavenDB
 
	Uma das aplicações usadas é o RavenDB. O RavenDB é um sistema de base de dados de documentos implementado sobre a Framework .NET. É uma solução transacional, que armazena os dados no formato JSON e que para leitura e escrita de dados suporta a utilização da componente Linq da Framework .NET ou de uma API RESTful disponibilizada através do protocolo HTTP.
	
	Internamente, o RavenDB usa índices Lucene que são criados automaticamente com a utilização do sistema ou criados explicitamente pelo programador através de expressões Map-Reduce, de forma a otimizar os acessos aos documentos.
		
		5.1.1. NoSql

 5.2. Knockout - fampinheiro

 5.3. Outras

		5.3.1. AttributeRouting - fampinheiro

	    5.3.2. AutoMapper
        ...
        Os objectos obtidos através de pedidos à base de dados são <mapeados> em objectos de dominio. Para facilitar o <mapeamento> entre objectos foi usada a biblioteca AutoMapper. 

        Ao configurar a biblioteca sobre a forma como um objecto pode ser <mapeado> noutro esta será capaz de fazer esse processo automáticamente, com a chamada ao método Map. Por exemplo:
    `Project project = Mapper.Map<ProjectDto, Project>(projectDto)`

6.	Trabalho Futuro / Alteração Planeamento


