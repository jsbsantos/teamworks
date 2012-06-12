Domínio
=

Para representar os requisitos e funcionalidades pretendidas para resolver o problema identificado é descrito nesta secção o modelo de domínio usado na infra-estrutura.

Para a elaboração e descrição do modelo de domínio utiliza-se a abordagem [Domain-Driven Design](#ddd) e os seus conceitos fundamentais, que podem ser consultados no [anexo1](#), presente no final do documento.

De forma a ilustrar as diversas interacções possíveis que os utilizadores podem ter com a infra-estrutura desenvolvida neste projecto, são apresentados os seguintes diagramas *use case* (casos de utilização) e uma breve descrição dessas interacções.

As figuras [usecaseprojecto](#) e [usecasetarefa](#) representam os casos de utilização de um membro com permissões de escrita (administração) ao nível de um projecto.

![Projecto - Caso de utilização de um membro com permissão de escrita (administração) \label{usecaseprojecto}](https://dl.dropbox.com/s/74grwphgl5m8me7/usecaseprojecto.png)

Um membro com permissões de escrita (administração) num projecto pode efectuar sobre este as seguintes acções:

* uma

* duas 

* três

![Tarefa - Caso de utilização de um membro com permissão de escrita (administração) \label{usecasetarefa}](https://dl.dropbox.com/s/1se8rhskj43zt73/usecasetarefa.png)

Um membro com permissões de escrita (administração) numa tarefa pode efectuar sobre esta as seguintes acções:

* uma

* duas 

* três

A figura [usecaseuser](#) ilustra um caso de utilização de um membro de um projecto apenas com permissão de leitura.

![Projecto - Caso de utilização de um membro apenas com permissão de leitura\label{usecaseuser}](https://dl.dropbox.com/s/2qoxj6k8swb07ds/usecaseuser.png)

Um membro apenas com permissão de leitura num projecto pode efectuar sobre esta as seguintes acções:

* uma

* duas 

* três

Modelo
-
Tendo como base as acções ilustradas nos diagramas *use case* foram identificadas duas entidades centrais, a **pessoa** (Person) e o **projecto** (Project). Cada pessoa pode estar envolvida em vários projectos, assim como um projecto pode ser desenvolvido por várias pessoas. Estas entidades são descritas como:

* **Pessoa**, representada pelo nome de utilizador, o *email* e a *password*. O *email* é usado para comunicar com a pessoa e os outros dois atributos servem para autenticar o utilizado perante o sistema.

* **Projecto**, a raiz da infra-estrutura que agrega as membros (Pessoas) associados ao projecto.

No contexto de um projecto é ainda possível definir **Tarefas** (*Task*) que, assim como a pessoa e o projecto, são consideradas entidades de domínio. Uma tarefa tem um nome e uma descrição e pode ter várias pessoas associadas. A tarefa tem também uma previsão do tempo estimado para a sua realização (e.g. número de horas) e a data prevista de conclusão. As pessoas associadas a uma tarefa podem registar o tempo que usaram na sua realização (*TimeEntry*).

Sobre os projecto e tarefas é ainda possível criar **Debates** (Discussions), visíveis a todos os utilizadores que lhes estão associados, onde estes podem colocar **Mensagens** (*Message*).

A descrição destas entidades e das suas relações é descrita no seguinte diagrama de classes: 

![Diagrama UML de classes da solução\label{uml}](https://dl.dropbox.com/s/z646fu75gf71mwq/uml.png)

No modelo de domínio da implementado foram definidos como agregados os objectos de domínio **Projecto** e **Pessoa**; **Tarefa** e **Debate** são considerados como entidades porque possuem um identificador único no sistema; o **Registo de Tempo** e **Message* são definidos como *value object*.


Segurança
-

### Registo 

Para utilizar a aplicação é necessário que o utilizador se registe. No registo o utilizador indica o email, usado pela aplicação como forma de comunicar com ele; o nome de utilizador para o identificar; e a password que em conjunto com o nome de utilizador é usada para autenticar o utilizador.

Na base de dados é guardado o *hash* da *password*.
O *hash* é gerado usando um algoritmo de dispersão (SHA-256) que tem como entrada a concatenação da password com um *salt* aleatório.

### Autenticação

A autenticação na aplicação é feita usando o nome de utilizador e a *password*. O nome de utilizador é usado para obter a instância de Person que representa o utilizador a autenticar (people/nome-de-utilizador). A autenticação é valida se o resultado da função de dispersão usada no registo for igual ao obtido usando os dados inseridos pelo utilizador.

A função de dispersão na autenticação tem como parâmetro de entrada a concatenação da *password* inserida com o *salt* presente na instância de Person obtida, *salt* gerado no momento em que o utilizador se registou.

### Autorização

Os utilizadores da infra-estrutura podem fazer todas as acções desde que tenham autorização para a fazer. As restrição são impostas dependendo da(s) entidade(s) com que o utilizador que interagir. Um utilizador só pode aceder a uma entidade se previamente tiver sido adicionado pelo criador da entidade pois este fica automaticamente associado à entidade que criou.
