Domínio
=

De forma a ilustrar as diversas interacções possiveis que os utilizadores podem ter com a infra-estrutura desenvolvida neste projecto, são apresentados os seguintes diagramas *use case* (caso de utilização). A figura **[usecaseprojecto]()** e **[usecasetarefa]()** representam os casos de utilização de um membro com permissões de escrita ao nível de um projecto.

<!---figure-->
<!---T:centering-->

![Projecto - Caso de utilização de um membro com permissão de escrita](https://dl.dropbox.com/s/74grwphgl5m8me7/usecaseprojecto.png)<!--- usecaseprojecto -->

<!---!figure-->
<!---T:FloatBarrier-->

<!---figure-->
<!---T:centering-->

![Tarefa - Caso de utilização de um membro com permissão de escrita](https://dl.dropbox.com/s/1se8rhskj43zt73/usecasetarefa.png)<!--- usecasetarefa -->

<!---!figure-->
<!---T:FloatBarrier-->

A figura **[usecaseuser]()** ilustra um caso de utilização de um membro de um projecto, **sem** permissões de administração.

<!---figure-->
<!---T:centering-->

![Projecto - Caso de utilização de um membro apenas com permissão de leitura](https://dl.dropbox.com/s/2qoxj6k8swb07ds/usecaseuser.png)<!--- usecaseuser -->

<!---!figure-->
<!---T:FloatBarrier-->

Modelo de Domínio
-
Para a elaboração e descrição do modelo de domínio usaram-se conceitos de Domain-Driven Design (ddd)<!---cite-->, que podem ser consultados no Anexo I, presente no final do documento.

Tendo como base as acções ilustradas nos diagramas *use case* foram identificadas duas entidades centrais, a **pessoa** (Person) e o **projecto** (Project). Cada pessoa pode estar envolvida em vários projectos, assim como um projecto pode ser desenvolvido por várias pessoas. Estas entidades são descritas como:

* **Pessoa**, representada pelo nome de utilizador, o *email* e a *password*. O *email* é usado para comunicar com a pessoa e os outros dois atributos servem para autenticar o utilizado perante o sistema.

* **Projecto**, a raiz da infra-estrutura que agrega as membros (Perssoas) associados ao projecto.

No contexto de um projecto é ainda possível definir **Tarefas** (*Tasks*) que, assim como a pessoa e o projecto, são consideradas entidades de domínio. Uma tarefa tem um nome e uma descrição e pode ter várias pessoas associadas. As pessoas associadas a uma tarefa podem registar o tempo que usaram na sua realização (*TimeEntry*). A tarefa tem também uma previsão do tempo estimado para a sua realização (e.g. número de horas) e a data prevista de conclusão.

Sobre os projecto e tarefas é ainda possivel criar **Debates** (Discussions), visiveis a todos os utilizadores que lhes estão associados. 

A descrição destas entidades e das suas relações é descrita no seguinte diagrama de classes: 
# PRECISA DE UPDATE #

<!---figure-->
<!---T:centering-->

![Diagrama de classes da solução](https://dl.dropbox.com/s/z646fu75gf71mwq/uml.png)<!--- uml -->

<!---!figure-->
<!---T:FloatBarrier-->

No modelo de domínio da implementado foram definidos como agregados as entidades **Projecto** e **Pessoa**. Os objectos de domínio **Tarefa** e **Debates** também são considerados como entidades porque possui um identificador único no sistema. O objecto de domínio **Registo de Tempo** é definido como *value object*.


Segurança
-
