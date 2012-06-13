Descrição Geral
=

A solução *Teamworks* tem como objectivo disponiblizar funcionalidades para a gestão de projectos.

<span style="background-color: yellow">Funcionalidades</span>

* **Gestão de tarefas**

* **Registo de tempo**, interface para  registo do tempo despendido na realização de cada tarefa.

* **Debates**, local para troca de ideias, definição de requisitos e discussão sobre o projecto.

* **Repositório**, usado para guardar toda a informação e documentação associada ao projecto. Desta forma os dados não estão dispersos e são de fácil acesso aos elementos do projecto.

* **Monitorização**, possibilidade de monitorizar toda a informação produzida pelas funcionalidades anteriores. 

Casos de Utilização
-

A autenticação na infra-estrutura é mandatória para a sua utilização sendo que as únicas acções possíveis a um utilizador anónimo são o registo e a autenticação, como se pode ver pela figura [usecaseanonimo](#).

![Caso de utilização de utilizador não autenticado.\label{usecaseanonimo}](http://www.lucidchart.com/publicSegments/view/4fd71023-3b68-497b-b199-60a50a443549/image.png)

Depois de autenticado o utilizador tem a opção de criar um projecto ou alterar um projecto já existente. 
Pode adicionar utilizadores ao projecto, remover utilizadores, adicionar tarefas e apagar o projecto (figura [usecaseprojecto](#)). 

![Caso de utilização de um utilizador autenticado sobre o recurso projecto.\label{usecaseprojecto}](http://www.lucidchart.com/publicSegments/view/4fd87fb4-4998-40d0-b9bb-647a0a4022d4/image.png)

Depois de criar uma tarefa são disponibilizadas novas acções ao utilizador

![Caso de utilização de um utilizador autenticado sobre uma tarefa a que está associado\label{usacasetarefa}](http://www.lucidchart.com/publicSegments/view/4fd880af-0ac4-441f-881a-73300a443549/image.png)

Para aceder á infra-estrutura o utilizador pode fazê-lo através de um browser, um cliente ReST ou outro cliente que comunique com a api. 

Arquitectura
-

Na arquitetura da solução destacam-se três componentes a aplicação web, a web api e a camada de serviços. 

Estes elementos interagem entre si da seguinte forma (figura [arquitectura](#)): a camada de serviços é responsável pela gestão de dados e toda a lógica da aplicaçãoa; e a aplicação web e a web api usam a camada de serviços para responder aos pedidos que lhes são feitos. Na figura [arquitectura](#) pode observar-se 

A aplicação web é responsável 

![Arquitetura da infra-estrutura implementada\label{arquitectura}](http://www.lucidchart.com/publicSegments/view/4fd7611a-c4c0-4cf9-8a0a-34fc0adcb320/image.png)

Os elemenentos da arquitectura expostos ao utilizador são a aplicação web e a web api. 

