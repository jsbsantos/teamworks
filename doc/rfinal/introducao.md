Introdução
=

\label{sec:introducao}

O projecto descrito neste documento tem como objectivo agilizar a gestão de um projecto através de uma infra-estrutura *online*. Neste contexto um projecto é um esforço temporário para criar um produto, serviço ou qualquer outro resultado.
Para além da gestão do projecto é também importante gerir as pessoas que dele fazem parte.
A comunicação entre membros de um projecto pode ter um grande impacto na qualidade e velocidade com que o produto final é entregue. Desta forma a plataforma desenvolvida, para além da gestão de projectos, também possibilita a gestão dos seus membros, facilitando a comunicação e partilha de informação entre eles.

Para atingir os objectivos propostos para a realização de um projecto é necessária uma gestão eficiente em áreas como a organização, a utilização de tempo, o planeamento, organização, controlo de recursos e monitorização.  
A motivação para realização deste projecto surgiu porque as soluções de *software* existentes ou não oferecem todas as funcionalidades necessárias ou são demasiado complexas. 

Sendo este um projecto académico é oportunidade para alargar conhecimento e desenvolver competências em áreas não abordadas no decorrer do curso. Com isto em mente, um dos objectivos do projecto foi implementar a camada de dados utilizando uma base de dados não relacional e 
sempre que possível utilizar projectos *open source* para resolver problemas não decorrentes dos objectivo do projecto. A utilização de diferentes projectos *open source* levantam problemas de integração que têm de ser resolvidos.

Soluções Existentes
-

As soluções existentes que pretendem agilizar a gestão de projectos e podem ser encontradas sobre a forma de aplicações *desktop ou web*. 

As aplicações desktop são usadas em ambiente local limitando a interacção e partilha de resultados com outras pessoas do projecto.
Um exemplo deste tipo de aplicações é o *Microsoft Project* \cite{mproject} usado para o planeamento de tarefas, estimar a sua duração e atribuir-lhe recursos. 

Ao contrário das aplicações desktop todas as interacções com uma aplicação web ficam alojadas na *cloud* tornando a informação acessível a todos as pessoas do projecto. Há uma grande variedade de aplicações web disponíveis, entre as quais, se destacam as aplicações *basecamp* \cite{basecamp}, *freckle* \cite{freckle}, *trello* \cite{trello} e *LiquidPlanner* \cite{liquidplanner}.

A aplicação web *basecamp* foca as suas funcionalidades na partilha e armazenamento de informação dos projectos criados. Tem ainda gestão de actividades, *todos* e eventos. 
A aplicação *trello*  tem as mesmas funcionalidades enunciadas mas é uma solução sem custos de utilização.

A aplicação web *freckle* tem como funcionalidades principais o registo de horas e monitorização de tempo despendido por tarefa.

A aplicação web *LiquidPlanner* oferece as funcionalidades das anteriores e ainda o planeamento de tarefas mas com uma interface mais complexa que as soluções anteriores, o que aumenta o tempo despendido na gestão do projecto. 

Solução Teamworks
-

A solução *Teamworks* é composta por uma aplicação web e por uma web Api. A Api expõe todas os recursos da infra-estrutura permitindo o seu acesso através de diversos clientes (e.g. browser, aplicações móveis, aplicações desktop).
A aplicação web tem uma interface simples com o objectivo de minimizar o tempo utilizado na plataforma.

As funcionalidades disponibilizadas permitem a gestão de projectos e pessoas. 
Para a gestão de projectos é possível planear tarefas, estimar a sua duração, atribuir-lhes responsáveis e monitorizar o estado global do projecto. 
As pessoas podem ver as tarefas que têm a realizar e registar o tempo despendido. Para promover a colaboração e partilha de informações é possível pessoas do mesmo projecto e/ou da mesma tarefa criar discussões.

Organização do documento
-

O restante documento está organizado da seguinte forma. 
O capítulo 2 apresenta os casos de utilização e a arquitectura da solução e nos capítulos seguintes são descritos o domínio do problema e o modelo de dados. 
A partir do capítulo 5 é descrita de forma mais detalhada a implementação da infra-estrutura, os serviços, a web Api e a aplicação web.
