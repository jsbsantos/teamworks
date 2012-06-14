Descrição Geral
=

\label{sec:descricao-geral}

Como foi dito anteriormente, a solução *Teamworks*, desenvolvida neste projecto, tem como objectivo disponiblizar funcionalidades para a gestão de projectos. A cada projecto estão associadas as seguintes funcionalidades:

* **Gestão de tarefas**, interface para criação e gestão de tarefas, associadas a um projecto

* **Registo de tempo**, interface para  registo do tempo despendido na realização de cada tarefa.

* **Debates**, local para troca de ideias, definição de requisitos e discussão sobre o projecto.


Casos de Utilização
-

A autenticação na infra-estrutura é obrigatória para a sua utilização sendo que as únicas acções possíveis a um utilizador anónimo são o registo e a autenticação, como indicado na figura [usecaseanonimo]().

![Caso de utilização de utilizador não autenticado.\label{usecaseanonimo}](http://www.lucidchart.com/publicSegments/view/4fd71023-3b68-497b-b199-60a50a443549/image.png)

Depois de autenticado o utilizador tem a opção de criar um projecto ou alterar um projecto já existente. A um projecto já existente é possível adicionar e remover utilizadores, adicionar tarefas ou apagar o projecto (figura [usecaseprojecto]()). Ao criar um projecto o utilizador fica associado a ele e pode fazer todas as acções enunciadas. 

Ao criar uma tarefa são disponibilizadas novas acções ao utilizador. O utilizador pode, sobre a tarefa criada, criar novas entradas de registo do tempo dispendido na sua realização, adicionar e remover utilizadores e apagá-la. 

De forma a promover a troca e partilha de informação é possível aos utilizadores criar e aceder aos debates relacionados com os projectos e/ou tarefas em que estão envolvidos.

Os casos de utilização enunciados podem ser observados na figura [usecase]()

![Casos de utilização de um utilizador autenticado\label{usecase}](http://www.lucidchart.com/publicSegments/view/4fda0b7b-a694-44fe-85d8-4de80adcb320/image.png)

Para aceder á infra-estrutura o utilizador pode fazê-lo através de um browser ou um cliente que comunique com a api web. 

Arquitectura
-

Na arquitetura da solução destacam-se quatro componentes: a aplicação web, a web api, a camada de serviços e de dados. Os elemenentos da arquitectura expostos ao utilizador são a aplicação web e a web api.

Estes elementos interagem entre si da seguinte forma: a camada de serviços é responsável pela gestão de dados e toda a lógica da aplicação; a camada de dados é responsável pela persistência de dados; a aplicação web e a web api usam a camada de serviços para responder aos pedidos que lhes são feitos. A interacção entre estes elementos é ilustrada na figura [arquitectura]().

![Arquitetura da infra-estrutura implementada\label{arquitectura}](http://www.lucidchart.com/publicSegments/view/4fd9ee2c-c028-4828-8962-51ad0a4022d4/image.png)


