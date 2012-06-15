Descrição Geral
=

\label{sec:descricao-geral}

Como foi dito anteriormente, a solução *Teamworks*, desenvolvida neste projecto, tem como objectivo disponibilizar funcionalidades para a gestão de projectos. A cada projecto estão associadas as seguintes funcionalidades:

+ Possibilidade de criar tarefas e gerir o tempo da sua execução. Possibilidade ainda de atribuir reponsáveis pela sua realização e disponibilizar uma lista a cada utilizador para que possa facilmente dar prioridade às tarefas e organizar o seu tempo. 

+ Possibilidade de cada utilizador registar o tempo que despendeu em determinada tarefa. Esta funcionalide é importante para controlar se as estimativas estão correctas e se o desenvolvimento do projecto está a correr como esperado.

+ Para promover a colaboração é possivel aos utilizadores um àrea de "debate" onde os partilharem informação, trocar ideias e debater soluções em áreas diponíveis no projecto e nas tarefas.

+ Ferramentas para monitorização do estado do projecto.

Para além das funcionalidades enunciadas o acesso à informação dos utilizadores e dos projectos deve ser mantida apenas acessível aos utilizadores com quem tenha sido partilhada.


Casos de Utilização
-

A autenticação na infra-estrutura é obrigatória para a sua utilização sendo que as únicas acções possíveis a um utilizador anónimo são o registo e a autenticação, como indicado na figura \ref{fig:usecase-anonimo}

![Caso de utilização de utilizador não autenticado.\label{fig:usecase-anonimo}](http://www.lucidchart.com/publicSegments/view/4fd71023-3b68-497b-b199-60a50a443549/image.png)

Depois de autenticado o utilizador tem a opção de criar um projecto ou alterar um projecto já existente. A um projecto já existente é possível adicionar e remover utilizadores, adicionar tarefas ou apagar o projecto. Ao criar um projecto o utilizador fica associado a ele e pode fazer todas as acções enunciadas. 

Ao criar uma tarefa são disponibilizadas novas acções ao utilizador. O utilizador pode, sobre a tarefa criada, criar novas entradas de registo do tempo despendido na sua realização, adicionar e remover utilizadores e apagá-la. 

De forma a promover a troca e partilha de informação é possível aos utilizadores criar e aceder aos debates relacionados com os projectos e/ou tarefas em que estão envolvidos.

Os casos de utilização enunciados podem ser observados na figura \ref{fig:usecase-global}

![Casos de utilização de um utilizador autenticado\label{fig:usecase-global}](http://www.lucidchart.com/publicSegments/view/4fda0b7b-a694-44fe-85d8-4de80adcb320/image.png)

Para aceder á infra-estrutura o utilizador pode fazê-lo através de um browser ou de um cliente da Api. 

Arquitectura
-

Na arquitetura da solução destacam-se quatro componentes, a aplicação web, a web Api[^api], a camada de serviços e a de dados, todos eles implementados utilizando a *framework* .NET \cite{net}. A aplicação web utiliza processamento do lado do cliente para complementar a implementação do servidor.

[^api]: A web Api é referida como Api no resto do documento.

A aplicação web e a Api são implementadas utilizando a *framework* ASP.NET \cite{aspnet} e expõem as funcionalidades da infra-estrutura através do protocolo HTTP. A camada de serviços é responsável por toda a lógica da infra-estrutura e a camada de dados tem como responsabilidade persistir os dados e disponibilizá-los quando pedidos. Para a persistência dos dados é usada a base de dados de documentos RavenDB \cite{ravendb}.

A interação dos componentes é a seguinte: a aplicação web e a Api usam a camada de serviços para responder aos pedidos que lhes são feitos; a camada de serviços envia e obtem dados da camada de dados para implementar a sua lógica; e a camada de dados é responsável pela comunicação com a base de dados. A figura \ref{fig:arquitetura} demonstra esta interação.

![Arquitetura da infra-estrutura *Teamworks*\label{fig:arquitetura}](http://www.lucidchart.com/publicSegments/view/4fd9ee2c-c028-4828-8962-51ad0a4022d4/image.png)

Alguns dos problemas não relacionadas com a infra-estrutura são resolvidos usando projectos *opensource* que serão indicados quando se justificar.
