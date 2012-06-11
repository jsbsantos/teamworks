Introdução
=

Um projecto é uma actividade em grupo ou individual com o proposito e objectivo definido.
Qualquer pessoa está envolvida em projectos no contexto profissional e/ou pessoal e o aproveitamento do tempo dispendido em cada um é uma preocupação constante resolvida por uma boa gestão de projectos. 

A gestão de um projecto não se resume à organização de tempo. Gestão de projecto é planeamento, organização, controlo de recursos e monitorização.

Este projecto tem como objectivo a realização de uma infraestrutura de apoio à gestão de projectos. A infraestrutura disponibiliza funcionalidades para planear, atribuir recursos e registar o tempo dispendido nas tarefas, assim como, ferramentas para partilhar informação e opiniões sobre cada projecto criado.

A motivação para a realização do projecto descrito no presente documento surgiu porque as soluções de *software* existentes ou não oferecem todas as funcionalidades necessárias ou são demasiado complexas.

Soluções Existentes
-

Actualmente existem várias soluções que pretendem agilizar a gestão de projectos e podem ser encontradas sobre a forma de uma aplicação desktop ou aplicação web. 

As aplicações desktop são usadas num ambiente local ficando o resultado da interação com a aplicação alojado no computador pessoal de quem a usa. Um exemplo deste tipo de aplicações é a aplicação *Microsoft Project* usada para o planeamento de tarefas, estimar a sua duração e atribuir-lhe recursos. 

Ao contrário das aplicações desktop todas as interações com uma aplicação web ficam alojadas na *cloud* tornando a informação acessível a todos os membros do projecto. Há uma grande variedade de aplicações web disponíveis, entre as quais, se destacam as aplicações *basecamp*, *freckle* e *LiquidPlanner*.

A aplicação *basecamp* foca as sua funcionalidades na partilha e armazenamento de informação dos projectos criados, e tem ainda gestão de actividades, *todo*s e eventos.

Na abordagem oposta a aplicação *freckle* tem como funcionalidades proncipais o registo de horas e monitorização de tempo dispendido por tarefa.

A solução *LiquidPlanner* oferece as funcionalidades das anteriores e ainda o planeamento de tarefas mas com uma interface mais complexa onde o tempo dispendido a gerir o projecto é superior. 

Solução Teamworks
-

Este projecto tem como objectivo desenvolver uma infra-estrutura que forneça funcionalidades de gestão e monitorização de projectos durante o seu ciclo de vida, como o debate de soluções, definição de tarefas, planeamento, atribuição de responsáveis e registo de tempo despendido na sua realização. As funcionalidades a desenvolver são:

* **Registo de tempo**, interface para  registo do tempo despendido na realização de cada tarefa.

* **Debates**, local para troca de ideias, definição de requisitos e discussão sobre o projecto.

* **Repositório**, usado para guardar toda a informação e documentação associada ao projecto. Desta forma os dados não estão dispersos e são de fácil acesso aos elementos do projecto.

* **Monitorização**, possibilidade de monitorizar toda a informação produzida pelas funcionalidades anteriores. 

A solução desenvolvida deve ser acessível através de diversas plataformas (e.g. browser, aplicações de dispositivos móveis) e permitir a integração. 
Tendo em conta este requesito não é sensato criar soluções distintas para cada situação, a abordagem seguida passa por criar uma Web Api onde diferentes cliente cpodem comunicar com a infra-estrutura.

Como forma de demonstrar a criação de um cliente o desenvolvimento de uma aplicação Web sobre a Api é também um requesito

Organização do documento
-

