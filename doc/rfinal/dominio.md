Domínio
=

\label{sec:dominio}

Para descrever o modelo de domínio é utilizada uma abordagem *Domain-Driven Design* \cite{domaindrivendesign} (descrita no anexo \ref{app:domaindrivendesign}). Como base para a elaboração do modelo de domínio foram usadas as características e funcionalidades apresentadas na  descrição geral (ver secção \ref{sec:descricao-geral}).

Modelo de Domínio
-

\label{sec:dominio-modelo}

As entidades raiz identificadas são **pessoa** (`Person`) e **projecto** (`Project`). Cada pessoa pode estar envolvida em vários projectos, e um projecto pode ter várias pessoas envolvidas. 

A entidade **pessoa** é representada pelo nome de utilizador, *email* e *password*.
O *email* é usado para comunicar com a pessoa e os outros dois atributos para autenticar o utilizador. A entidade **projecto** agrega as pessoas que lhe estão associadas sendo possível definir **actividades** que, como **pessoa** e **projecto**, são entidades do domínio.

Uma **actividade** (`Activity`) tem nome e descrição e, pode também, ter várias pessoas associadas. Para além destes atributos, tem ainda o tempo estimado para a sua realização (e.g. número de horas), a data prevista de conclusão e **registos de tempo** (`Timelog`) despendido na sua realização, que podem ser adicionados pelas **pessoas** associadas.

Sobre os projectos e tarefas é ainda possível criar **debates** (`Discussion`), visíveis apenas a **pessoas** que lhes estão associadas, onde é possível criar **mensagens** (`Message`).

As entidades e as suas relações estão representadas no diagrama de classes da figura \ref{fig:diagramadedominio}.

![Diagrama classes e representação da relação entre os objectos de domínio.\label{fig:diagramadedominio}](http://www.lucidchart.com/publicSegments/view/4fd89208-da90-4b53-8506-66290a443549/image.png)

Depois de analisadas as relações e responsabilidades de cada entidade foram caracterizados como agregados os objectos de domínio **projecto** e **pessoa**.
Os objectos **actividade** e **debate** são considerados entidades porque são identificados univocamente.
A **actividade** é entidade do agregado projecto e o **debate** pertence tanto ao agregado projecto como à entidade actividade.
O **registo de tempo** e as **mensagens** são *value object* e inseridos nas entidades que os referenciam. 

Segurança
-

\label{sec:dominio-seguranca}

Por questões de segurança todas as acções na infra-estrutura têm de ser feitas por utilizadores autenticados sendo por isso necessário disponibilizar forma dos utilizadores se registarem. 
Para manter a privacidade dos dados tem ainda de haver, aliado à autenticação, políticas de acesso e autorização.

No registo o utilizador indica o email, utilizado como meio de comunicação; o nome de utilizador para o identificar; e a password, que em conjunto com o nome de utilizador, é usada para autenticar o utilizador.
Esta informação é persistida na base de dados, à excepção da *password* da qual é apenas persistido o *hash*.
O *hash* da password é gerado usando um algoritmo de dispersão (*SHA-256*) que tem como entrada a concatenação da password com um *salt*[^salt] aleatório.

A autenticação é feita usando o nome de utilizador e a *password* e é válida se o resultado da função de dispersão, usada no registo para a *password* for igual ao obtido usando os dados inseridos pelo utilizador.
A função de dispersão, na autenticação, tem como parâmetro de entrada a concatenação da *password* inserida com o *salt* presente na instância de *Person* obtida.

A política de acesso é definida individualmente por cada entidade. Sendo que um utilizador só lhe pode aceder se tiver sido criada por si ou se lhe tiverem atribuído permissões.

[^salt]: O *salt* é utilizado para criar aleatoriedade na *password*. Desta maneira o *hash* gerado para a mesma *password* é diferente por utilizador.
