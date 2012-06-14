Introdução
=

\label{sec:introducao}

Actualmente é frequente qualquer pessoa estar envolvida em projectos, num contexto profissional e/ou pessoal, em que o aproveitamento do tempo despendido em cada um é uma preocupação constante. Esta preocupação pode ser atunuada com uma gestão eficaz desses projectos. 
A gestão de um projecto não se resume à organização de tempo.
Gestão de projecto é planeamento, organização, controlo de recursos e monitorização.
 
Este projecto tem como objectivo a realização de uma infra-estrutura de apoio à gestão de projectos. A infra-estrutura disponibiliza funcionalidades para planear, atribuir recursos e registar o tempo despendido nas suas tarefas, assim como ferramentas para partilhar informação e opiniões sobre os projectos criados.

A motivação para a realização do projecto descrito no presente documento surgiu porque as soluções de *software* existentes ou não oferecem todas as funcionalidades necessárias ou são demasiado complexas.

Soluções Existentes
-

Actualmente existem várias soluções que pretendem agilizar a gestão de projectos e podem ser encontradas sobre a forma de uma aplicação desktop ou aplicação web. 

As aplicações desktop são usadas num ambiente local ficando o resultado da interacção com a aplicação alojado no computador pessoal de quem a usa. Um exemplo deste tipo de aplicações é a aplicação *Microsoft Project* usada para o planeamento de tarefas, estimar a sua duração e atribuir-lhe recursos. 

Ao contrário das aplicações desktop todas as interacções com uma aplicação web ficam alojadas na *cloud* tornando a informação acessível a todos os membros do projecto. Há uma grande variedade de aplicações web disponíveis, entre as quais, se destacam as aplicações *basecamp*, *freckle* e *LiquidPlanner*.

A aplicação web *basecamp*^[*Basecamp* - http://basecamp.com] foca as sua funcionalidades na partilha e armazenamento de informação dos projectos criados. Tem ainda gestão de actividades, *todos* e eventos.

A aplicação web *freckle*^[*Freckle* - http://letsfreckle.com/] tem como funcionalidades principais o registo de horas e monitorização de tempo despendido por tarefa.

A aplicação web *LiquidPlanner*^[*LiquidPlanner* - http://www.liquidplanner.com/] oferece as funcionalidades das anteriores e ainda o planeamento de tarefas mas com uma interface mais complexa que as soluções anteriores, o que aumenta o tempo despendido na gestão do projecto. 

Solução Teamworks
-

Este projecto tem como objectivo desenvolver uma infra-estrutura que forneça funcionalidades de gestão e monitorização de projectos durante o seu ciclo de vida, como o debate de soluções, definição de tarefas, planeamento, atribuição de responsáveis e registo de tempo despendido na sua realização. 

A solução desenvolvida deve ser acessível através de diversas plataformas (e.g. browser, aplicações de dispositivos móveis) e permitir a integração com aplicações externas. Tendo em conta este requisito não é sensato criar soluções distintas para cada situação, por isso a abordagem seguida passa por criar uma web api que diferentes clientes podem usar para comunicar com a infra-estrutura e uma aplicação web que usa essa mesma api.

Pretende-se ainda que a interface com o utilizador, disponibilizada através de uma aplicação web, seja simples de forma a minimizar o tempo necessário para gestão dos seus projectos.
