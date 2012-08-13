Descrição Geral
=

\label{sec:descricao-geral}


Como indicado anteriormente, a solução *Teamworks*, tem como objectivo disponibilizar funcionalidades para a gestão de projectos e pessoas. Para isso a cada projecto estão associadas as seguintes funcionalidades:

+ Possibilidade de criar actividades, gerir o tempo estimado da sua execução, atribuir responsáveis pela sua realização e disponibilizar uma lista a cada utilizador para que possa decidir a que actividades dar prioridade e organizar o seu tempo. 

+ Possibilidade de cada utilizador registar o tempo que despendeu em determinada actividade. Esta funcionalidade é importante para controlar se as estimativas estão correctas e se o desenvolvimento do projecto está a correr como esperado.

+ Ferramentas para monitorização do estado do projecto.

Para promover a colaboração existe ainda a possibilidade dos utilizadores terem uma área de "debate" onde partilham informação, trocam ideias e debatem soluções relacionadas com o projecto ou com actividades.

Para além das funcionalidades enunciadas o acesso à informação dos utilizadores e projectos deve estar disponível aos utilizadores com quem tenha sido partilhada.


Casos de Utilização
-

A autenticação na plataforma é obrigatória para a sua utilização, sendo que as únicas acções possíveis a um utilizador anónimo são o registo e a autenticação, como indicado na figura \ref{fig:usecase-anonimo}

![Caso de utilização de utilizador não autenticado.\label{fig:usecase-anonimo}](http://www.lucidchart.com/publicSegments/view/4fd71023-3b68-497b-b199-60a50a443549/image.png)

Depois de autenticado o utilizador tem a opção de criar um projecto ou alterar um projecto já existente. A um projecto já existente é possível adicionar e remover utilizadores, adicionar actividades ou apagar o projecto. Ao criar um projecto o utilizador fica associado a ele e pode fazer todas as acções enunciadas. 

Ao criar uma actividade são disponibilizadas novas acções ao utilizador. O utilizador pode, sobre a actividade criada, criar novas entradas de registo do tempo despendido na sua realização, adicionar e remover utilizadores e apagá-la. 

De forma a promover a troca e partilha de informação é possível aos utilizadores criar e aceder aos debates relacionados com os projectos e/ou actividades em que estão envolvidos.

Os casos de utilização enunciados podem ser observados na figura \ref{fig:usecase-global}

![Casos de utilização de um utilizador autenticado\label{fig:usecase-global}](http://www.lucidchart.com/publicSegments/view/4fda0b7b-a694-44fe-85d8-4de80adcb320/image.png)

O acesso á plataforma pode ser feito através de um browser ou de um cliente da Api.

Arquitectura
-

Na arquitectura da solução destacam-se quatro componentes, a aplicação web, a Api, a camada de serviços e a de dados, todos eles implementados utilizando a *framework* .NET \cite{net}.
A aplicação web utiliza processamento do lado do cliente para complementar a implementação do servidor.

A aplicação web e a Api são implementadas utilizando a *framework* ASP.NET \cite{aspnet} e expõem as funcionalidades da plataforma através do protocolo HTTP. 
A camada de serviços é responsável por toda a lógica aplicacional e a camada de dados tem como responsabilidade persistir os dados e disponibilizá-los quando pedidos. Para a persistência dos dados é usada a base de dados de documentos RavenDB \cite{ravendb}.

A interacção dos componentes é a seguinte: a aplicação web e a Api usam a camada de serviços para responder aos pedidos que lhes são feitos; a camada de serviços envia e obtém dados da camada de dados para implementar a sua lógica; e a camada de dados é responsável pela comunicação com a base de dados. A figura \ref{fig:arquitectura} demonstra esta interacção.

![Arquitectura da plataforma *Teamworks*\label{fig:arquitectura}](http://www.lucidchart.com/publicSegments/view/4fd9ee2c-c028-4828-8962-51ad0a4022d4/image.png)

Alguns dos problemas não directamente relacionadas com a plataforma são resolvidos usando projectos *opensource* que serão indicados quando se justificar.
