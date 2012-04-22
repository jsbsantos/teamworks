# Proposta de Projecto

# TeamWorks
> a solution  for project planning, managing and colaboration

## Palavras-chave
Planeamento de tarefas, gestão de tempo, colaboração, monitorização.

## Tipo de projecto
Projecto de desenvolvimento

## Problema
Auxiliar nas actividades associadas à gestão de um projecto.

## Introdução
Um projecto é um esforço temporário realizado por uma equipa, com o objectivo de criar um produto ou serviço. 
No decorrer de qualquer projecto existem preocupações que saem do seu domínio como o planeamento de tarefas, gestão de pessoas e tempo. Como exemplo dessa realidade tomemos a seguinte situação:

> Surge um novo requisito. É necessário planear as tarefas a ele associadas, estimar a sua duração e atribuir a sua realização a um elemento da equipa. Durante a sua realização são tomadas decisões e registado o tempo usado. 

Do caso enunciado surgem vários tipos de informação:

* Análise do requisito e documentação associada;
* O resultado do planeamento (pessoa(s) associada(s) à tarefa e duração estimada);
* Discussões sobre as opções tomadas;
* Registo detalhado do tempo despendido e tarefas realizadas.

Todas estes “extras” associados a um projecto consomem tempo e recursos. Esta informação deve ser guardada de forma a ser acessível por toda a equipa para que esta esteja inteirada das alterações e para referência futura. Uma forma de tornar estas tarefas mais eficientes e ter a informação sempre actualizada e acessível, passa por ter uma plataforma central que ofereça formas de agilizar os processos anteriormente descritos.

Actualmente existem várias aplicações que têm como objectivo mitigar estes problemas, algumas sob a forma de aplicações desktop e outras de aplicações web. 

## Análise
Para a resolução dos problemas introduzidos podem ser usadas as seguintes aplicações:

* A aplicação desktop [Microsoft Project](http://microsoft.com/project) oferece a funcionalidade de planeamento e monitorização de progresso de projectos; 
* A aplicação web [Basecamp](http://basecamp.com/) mantém registo de todas as discussões feitas durante o decorrer do projecto e documentos e, permite gerir listas de to-do, eventos e acessos;
* Também pode ser usada a aplicação web [freckle](http://letsfreckle.com/), que tem como funcionalidades o registo de tempo despendido em cada tarefa e a sua monitorização;

Todavia, estas aplicações apenas resolvem o problema parcialmente.
O objectivo deste projecto é criar uma plataforma web que aborde os problemas introduzidos, simplificando a gestão de um projecto, de forma a que o tempo seja usado para o desenvolvimento efectivo do mesmo.

A plataforma servirá como ponto central de comunicação entre a equipa, de forma a que todos os membros tenham acesso a informação detalhada e actualizada do estado do projecto. Na plataforma deve ser possível consultar os projectos em curso, o seu planeamento e o registo de trabalho realizado pela equipa. Tem de ser possível criar tarefas, definir a sua duração, prioridade e precedência. A cada tarefa atribuir pessoas responsáveis que registam o tempo utilizado na sua realização. Toda a informação deve estar disponível para análise para que a equipa possa adaptar os desenvolvimentos dependendo da evolução e das necessidades do projecto.

Para promover a cooperação entre a equipa pretende-se possibilitar a criação de um repositório partilhado, onde os membros possam armazenar e consultar documentos relacionados com o projecto. Será ainda possível que os membros criem debates para a discussão de ideias.

## Recursos necessários
* Computador pessoal;
* Ferramentas de desenvolvimento de software;
* Acesso à sala de projecto.
* Hosting ([Azure](http://www.windowsazure.com/en-us/) ou [AppHarbor](https://appharbor.com/))

## Plano
O planeamento é feito à semana e tem como data inicial o dia de entrega do presente documento, não estando contemplado o trabalho já realizado.

 **Semana** | **Inicio** | **Observações** |
-----|-----|-----|
1 | 19/03 | Entrega das propostas até 19 de Março |
2 | 26/03 | Modelo de dados e escolha de tecnologias |
3 | 9/04 | Tarefas e projectos |
4 | 16/04 | Utilizadores e registo de horas - live 0.1 |
5 | 23/04 | Discussões 1.0 (apenas texto) |
6 | 30/04 | Aplicação web (UI) |
7 | 07/05 | Relatório de progresso e apresentação individual |
8 | 14/05 | Politicas de acesso - live 0.2 |
9 | 21/05 | Discussões 2.0 (upload ficheiros) |
10 | 28/05 | Gestão emails |
11 | 04/06 | Discussões 3.0 (integração com email) |
12 | 11/06 | Versão beta - live 0.3 |
13 | 18/06 | Cartaz e versão beta |
14 | 25/06 | Planeamento |
15 | 2/07 | Dashboard - live 0.4 |
16 |9/07 |Monitorização|
17 |16/07 |API - live 0.5|
18 |23/07 |Testes|
19 |03/09 |Relatório final (versão inicial)|
20 |10/09 |Relatório final (revisto pelo orientador)|

O planeamento termina no dia 18 de Setembro de 2012, data correspondente ao prazo de entrega do relatório de da versão final deste Projecto. 

## Resultados esperados
No final deste projecto esperamos atingir os seguintes resultados:

* Relatório final do projecto;
* Aplicação web disponível para qualquer utilizador;
* API que possibilite o acesso a aplicações externas.