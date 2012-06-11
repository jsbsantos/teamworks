Introdução
=

Um projecto é uma actividade em grupo ou individual com o proposito e objectivo definido.
Qualquer pessoa está envolvida em projectos no contexto profissional e/ou pessoal e o aproveitamento do tempo dispendido em cada um é uma preocupação constante resolvida por uma boa gestão de projectos. 

A gestão de um projecto não se resume à organização de tempo. Gestão de projecto é planeamento, organização, controlo de recursos e monitorização.

Este projecto tem como objectivo a realização de uma infraestrutura de apoio à gestão de projectos. A infraestrutura disponibiliza funcionalidades para planear, atribuir recursos e registar o tempo dispendido nas tarefas e ferramentas para partilhar opiniões e informação de todos os projectos criados.

A motivação para a realização do projecto descrito no presente documento surgiu porque as soluções de *software* existentes ou não oferecem todas as funcionalidades necessárias ou são demasiado complexas.

Objectivos
-

Este projecto tem como objectivo desenvolver uma infra-estrutura que forneça funcionalidades de gestão e monitorização de projectos durante o seu ciclo de vida, como o debate de soluções, definição de tarefas, planeamento, atribuição de responsáveis e registo de tempo despendido na sua realização. As funcionalidades a desenvolver são:

* **Registo de tempo**, interface para  registo do tempo despendido na realização de cada tarefa.

* **Debates**, local para troca de ideias, definição de requisitos e discussão sobre o projecto.

* **Repositório**, usado para guardar toda a informação e documentação associada ao projecto. Desta forma os dados não estão dispersos e são de fácil acesso aos elementos do projecto.

* **Monitorização**, possibilidade de monitorizar toda a informação produzida pelas funcionalidades anteriores. 

A solução desenvolvida deve ser acessível através de diversas plataformas (e.g. browser, aplicações de dispositivos móveis) e permitir a integração.

Soluções Existentes
-

Actualmente existem várias soluções que pretendem agilizar a gestão de projectos e podem ser encontradas sobre a forma de uma aplicação desktop ou aplicação web. 

As aplicações desktop são usadas num ambiente local ficando o resultado da interação com a aplicação alojado no computador pessoal de quem a usa. Um exemplo deste tipo de aplicações é a aplicação *Microsoft Project* usada para o planeamento de tarefas, estimar a sua duração e atribuir-lhe recursos. 

Ao contrário das aplicações desktop todas as interações com uma aplicação web ficam alojadas na *cloud* tornando a informação acessível a todos os membros do projecto. Há uma grande variedade de aplicações web disponíveis, entre as quais, se destacam as aplicações *basecamp*, *freckle* e *LiquidPlanner*.


<span style="background-color: yellow">Tabela comparativa</span>

Solução Teamworks
-

Tendo em conta o requisito de que a infra-estrutura tem de estar disponivel no maior numero de dispositivos/formatos e não é sensato criar soluções distintas para cada situação a abordagem seguida passa por criar uma web api que disponibiliza todos os recursos disponíveis. 
Desta forma quando surge a necessidade de um novo cliente, este comunica com a api e tem acesso a todos os recursos da infra-estrutura. 

Como forma de demonstrar a criação de um cliente foi desenvolvida uma aplicação web que comunica com a web api.

Organização do documento
-

