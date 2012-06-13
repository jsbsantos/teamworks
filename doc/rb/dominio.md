Domínio
=

Para representar os requisitos e funcionalidades pretendidas para resolver o problema identificado é descrito nesta secção o modelo de domínio usado na infra-estrutura.

Para a definição do domínio do projecto foi utilizada uma abordagem [Domain-Driven Design](#ddd) descrita no [anexo1](#). Na elaboração do modelo foram usados os diagramas de casos de utilização apresentados na secção *Descrição Geral*.

Modelo
-

As entidades raiz identificadas são **pessoa** e **projecto**. Cada pessoa pode estar envolvida em vários projectos, e um projecto pode ter várias pessoas envolvidas. 

A entidade **pessoa** é representada por nome de utilizador, *email* e *password*.
O *email* é usado para comunicar com a pessoa e os outros dois atributos servem para autenticar o utilizado perante o sistema. A entidade **projecto** agrega as pessoas associadas a um projecto. No contexto de um projecto é possível definir **tarefas** que, como a **pessoa** e o **projecto**, são consideradas entidades de domínio.

Uma **tarefa** tem nome e descrição e várias pessoas associadas. Para além destes atributos, tem ainda o tempo estimado para a sua realização (e.g. número de horas) e a data prevista de conclusão. As pessoas associadas a uma tarefa podem adicionar **registos de tempo** dispendido na sua realização.

Sobre os projecto e tarefas é ainda possível criar **debates**, vísiveis apenas ás pessoas associadas ás entidades, onde é possível criar **mensagens**.

No modelo de domínio foram definidos como agregados os objectos de domínio **projecto** e **pessoa**; **tarefa** e **debate** são considerados entidades do agregado projecto porque possuem um identificador único no sistema; o **registo de tempo** e **mensagens** são definidos como *value object*. 

A descrição destas entidades e das suas relações é descrita no seguinte diagrama de classes da figura [diagramadeclassesdominio](#).

![Diagrama de relação entre os objectos de domínio\label{diagramadeclassesdominio}](http://www.lucidchart.com/publicSegments/view/4fd89208-da90-4b53-8506-66290a443549/image.png)
Segurança
-

### Registo 

Como dito anteriormente para utilizar a aplicação é necessário o registo do utilizador. No registo o utilizador indica o email, para a infra-estrutura comunicar com este; o nome de utilizador para o identificar; e a password que em conjunto com o nome de utilizador é usada para autenticar o utilizador.

Os dados são persistidos tal como adicionados à excepção da *password* pois é apenas guardado o *hash* da *password*. 
O *hash* é gerado usando um algoritmo de dispersão (SHA-256) que tem como entrada a concatenação da password com um *salt* aleatório.

### Autenticação

A autenticação na aplicação é feita usando o nome de utilizador e a *password*. O nome de utilizador é usado para identificar o utilizador a autenticar. A autenticação é valida se o resultado da função de dispersão usada no registo for igual ao obtido usando os dados inseridos pelo utilizador.

A função de dispersão na autenticação tem como parâmetro de entrada a concatenação da *password* inserida com o *salt* presente na instância de Person obtida, *salt* gerado no momento em que o utilizador se registou.

### Autorização

Os utilizadores da infra-estrutura podem fazer todas as acções desde que tenham autorização para a fazer. As restrições impostas dependem da(s) entidade(s) com que o utilizador interage. Um utilizador só pode aceder a uma entidade se previamente tiver sido adicionado pelo criador da entidade pois este fica automaticamente associado à entidade que criou.
