Domínio
=

\label{sec:dominio}

Para descrever o modelo de domínio foi utilizada uma abordagem *Domain-Driven Design* \cite{domaindrivendesign} (descrita no anexo \ref{app:domaindrivendesign}). As caracteristicas e funcionalidades apresentadas na  descrição geral (ver secção \ref{sec:descricao-geral}) serviram de base para a elaboração do modelo de domínio.

Modelo
-

\label{sec:dominio-modelo}

As entidades raíz identificadas são **pessoa** (`Person`) e **projecto** (`Project`). Cada pessoa pode estar envolvida em vários projectos, e um projecto pode ter várias pessoas envolvidas. 

A entidade **pessoa** é representada pelo nome de utilizador, *email* e *password*.
O *email* é usado para comunicar com a pessoa e os outros dois atributos servem para autenticar o utilizador. A entidade **projecto** agrega as pessoas que lhe estão associados e  é possível definir **tarefas** que, como **pessoa** e **projecto**, são entidades do domínio.

Uma **tarefa** (`Task`) tem nome e descrição e pode também ter várias pessoas associadas. Para além destes atributos, tem ainda o tempo estimado para a sua realização (e.g. número de horas) a data prevista de conclusão e **registos de tempo** (`Timelog`) despendido na sua realização que podem ser adicionados pelas **pessoas** associadas.

Sobre os projectos e tarefas é ainda possível criar **debates** (`Thread`), visíveis apenas a **pessoas** que lhes estão associadas, onde é possível criar **mensagens** (`Message`).

A descrição destas entidades e das suas relações é descrita no seguinte diagrama de classes da figura \ref{fig:digramadeclassesdedominio}.

![Diagrama de relação entre os objectos de domínio.\label{fig:diagramadeclassesdedominio}](http://www.lucidchart.com/publicSegments/view/4fd89208-da90-4b53-8506-66290a443549/image.png)

Depois de analisadas as relações e responsabilidades de cada entidade foram caracterizados como agregados os objectos de domínio **projecto** e **pessoa**. As entidades **tarefa** e **debate** são entidades do agregado **projecto** pois é possível identificá-las univocamente. O **registo de tempo** e as **mensagens** são definidos como *value object* e inseridos nas entidades que os referenciam. 

Segurança
-

\label{sec:dominio-seguranca}

Por questões de segurança todas as acções na infra-estrutura têm de ser feitas por utilizadores autenticados sendo por isso necessário disponibilizar forma dos utilizadores se registarem. Para manter a privacidade dos dados tem ainda de haver, aliado á autenticação, políticas de acesso e autorização.

No registo o utilizador indica o email, utilizado como meio de comunicação; o nome de utilizador para o identificar; e a password, que em conjunto com o nome de utilizador, é usada para autenticar o utilizador.
Os dados são persistidos na base de dados à excepção da *password* que apenas o seu *hash* é persistido.
O *hash* é gerado usando um algoritmo de dispersão (*SHA-256*) que tem como entrada a concatenação da password com um *salt*[^salt] aleatório.

A autenticação é feita usando o nome de utilizador e a *password*. A autenticação é valida se o resultado da função de dispersão usada no registo for igual ao obtido usando os dados inseridos pelo utilizador.
A função de dispersão, na autenticação, tem como parâmetro de entrada a concatenação da *password* inserida com o *salt* presente na instância de *Person* obtida e o *salt* gerado no momento em que o utilizador se registou.

A política de acesso é definida individualmente por cada entidade. Sendo que um utilizador só lhe pode aceder se tiver sido criada por si ou se lhe tiverem dadas permissões.

[^salt]: O *salt* é utilizado para criar aleatoriedade na *password*. Desta maneira o *hash* gerado para a mesma *password* é diferente por utilizador.