Descrição Geral
=

\label{sec:descricao-geral}

Como indicado anteriormente, a infra-estrutura *Teamworks*, tem como objectivo disponibilizar funcionalidades para a gestão de projectos e pessoas. 
As funcionalidades disponibilizadas são as seguintes:

+ Possibilidade de criar actividades, gerir o tempo estimado para a sua execução, atribuir responsáveis pela sua realização 

+ Disponibilizar uma lista a cada utilizador para que possa decidir a que actividades dar prioridade e organizar o seu tempo. 

+ Possibilidade de cada utilizador registar o tempo que despendeu em determinada actividade. Esta funcionalidade é importante para controlar se as estimativas estão correctas e se o desenvolvimento do projecto está a correr como esperado.

+ Ferramentas para monitorização do estado do projecto.

Para promover a colaboração existe ainda a possibilidade dos utilizadores terem uma área de discussão onde partilham informação, trocam ideias e debatem soluções relacionadas com o projecto ou com as actividades do projecto.

Para além das funcionalidades enunciadas só os utilizadores associados devem ter acesso à informação dos projectos, actividades e discussões.

Casos de Utilização
-

A autenticação na plataforma é obrigatória para todas as acções para além do registo e da autenticação, como indicado na figura \ref{fig:usecase-anonimo}.

![Caso de utilização de utilizador não autenticado.\label{fig:usecase-anonimo}](http://www.lucidchart.com/publicSegments/view/4fd71023-3b68-497b-b199-60a50a443549/image.png)

Depois de autenticado o utilizador tem a opção de criar um projecto. A um projecto já existente é possível adicionar e remover pessoas, adicionar actividades, alterar ou apagar o projecto. Ao criar um projecto o utilizador fica associado a este e tem a possibilidade de fazer todas as acções enunciadas.

Ao criar uma actividade são disponibilizadas novas acções ao utilizador. 
O utilizador pode, sobre a actividade criada, criar novas entradas de registo do tempo despendido na sua realização, adicionar e remover pessoas e apagá-la. 

De forma a promover a troca e partilha de informação é possível aos utilizadores criar e aceder aos debates relacionados com os projectos e/ou tarefas em que estão envolvidos.

Os casos de utilização enunciados podem ser observados na figura \ref{fig:usecase-global}.

![Casos de utilização de um utilizador autenticado\label{fig:usecase-global}](http://www.lucidchart.com/publicSegments/view/4fda0b7b-a694-44fe-85d8-4de80adcb320/image.png)

O acesso à plataforma pode ser feito através de um *browser* ou de um cliente da Api.

Arquitectura
-

Na arquitectura da solução destacam-se quatro componentes, a aplicação web, a Api, a camada de serviços e a de dados, todos eles implementados utilizando a *framework* .NET \cite{net}.
A aplicação web utiliza processamento do lado do cliente para complementar a implementação do servidor.

A aplicação web e a Api são implementadas utilizando a *framework* ASP.NET \cite{aspnet} e expõem as funcionalidades da plataforma através do protocolo HTTP. 
A camada de serviços é responsável por toda a lógica aplicacional e a camada de dados tem como responsabilidade persistir os dados e disponibilizá-los quando pedidos. Para a persistência dos dados é usada a camada de dados.

![Arquitectura da plataforma *Teamworks*\label{fig:arquitectura}](http://www.lucidchart.com/publicSegments/view/4fd9ee2c-c028-4828-8962-51ad0a4022d4/image.png)

A interacção dos componentes é a seguinte: a aplicação web e a Api usam a camada de serviços para responder aos pedidos que lhes são feitos; a camada de serviços envia e obtém dados da camada de dados para implementar a sua lógica; e a camada de dados é responsável pela comunicação com a base de dados. A figura \ref{fig:arquitectura} demonstra esta interacção.

