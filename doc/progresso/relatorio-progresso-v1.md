#1. Introdução

O presente documento constitui o relatório de progressodo projecto intitulado “teamworks”, desenvolvido no ambito da unidade curricular de Projecto e Seminário do ano lectivo 2011/2012. É realizado pelos alunos Filipe Pinheiro e João Santos e orientado pelo docente João Pedro Patriarca.

Este projecto tem como objectivo a disponibilização de uma infra-estrutura de apoio à realização e gestão de projectos oferecendo funcionalidades de planeamento, registo de horas e monitorização. Para fomentar a colaboração entre membros são também oferecidas funcionalidades que permitem discussão e partilha de informação. A infra-estrutura é composta por uma aplicação Web e um serviço Web que baseado na arquitectura ReST.

As funcionalidades passam pela criação de planos de trabalho, afectação de recursos a tarefas, registo do tempo despendido, monitorização do estado actual do projecto, áreas de discussão, partilha de informação, etc.

#2. Descrição Geral

Esta infra-estrutura é disponibilizada através de uma aplicação Web e um serviço Web RESTful que permitam efectuar todas as acções descritas anteriormente. É possível aos utilizadores criar projectos, tarefas, registo de tempo de trabalho, como ilustrado nas figuras 1 e 2.
____
![figura 1.1 - Caso de utilização de um administrador - Projecto](http://www.lucidchart.com/publicSegments/view/4fa552b3-78b8-4d25-bc24-37620a58bb74/image.png =150x150)
____
![figura 1.2 - Caso de utilização de um administrador - Tarefa](http://www.lucidchart.com/publicSegments/view/4fa55112-8018-47df-9e4d-1f9b0a8c1042/image.png =150x150)
____
![figura 2 - Caso de utilização de um utlizador](http://figura2.x)
____

##2.1. Modelo de Dados

Para solucionar o problema apresentado por este projecto foram identificadas duas entidades centrais, a **Pessoa** e o **Projecto**.
Cada Pessoa pode estar envolvida em vários Projectos, assim como um Projecto pode ser desenvolvido por várias Pessoas. Para representar esta realidade existe a noção de:

 * **Pessoa**, representada pelo nome de utilizador, o _email_ e a _password_. O _email_ é usado para comunicar com a pessoa e os outros dois atributos servem para autenticar o utilizador perante o sistema.
 * O **Projecto**,  é o a raíz da infra-estrutura e agrega as pessoas que a ele estão associados.

No contexto de um Projecto é possível definir **Tarefas**, que são também consideradas entidades de domínio. Uma Tarefa tem um nome e uma descrição. As Tarefas podem ter várias Pessoas associadas que podem registar o tempo usaram na sua realização. A Tarefa tem também uma previsão do tempo estimado para a sua realização e a data em que deve ser concluída.

A descrição destas entidades de das suas relações é descrita no seguinte diagrama UML:
![UML teamworks](http://www.lucidchart.com/publicSegments/view/4fa54e58-d8bc-4005-a258-131c0a8c1042/image.png =150x150)

#2.2. Modelo de Domínio

O modelo de domínio é o conjunto de vários termos, diagramas e conceitos que representam a informação e o comportamento de uma aplicação face a um problema. Para representar o modelo de domínio podem ser usados diagramas UML, texto detalhado, esquemas entidade-associação, use cases, etc.
entre si. Um aspecto importante do modelo de domínio é a sua compreensão por todos os intervenientes no projecto (e.g. arquitectos de software, programadores, cliente). Aos elementos de um modelo de domínio dá-se o nome de objectos de domínio.

Entidades, _value objects_, agregados, fábricas e repositórios são alguns conceitos transversais a qualquer modelo de domínio.
Uma entidade representa um objecto do modelo que tem um identificador único em todo o seu tempo de vida na aplicação e pode ser acedido através desse identificador. 

Um _value object_, assim como uma entidade, é representado pelas suas características e atributos mas não tem identidade no sistema, ou seja _value object_ com os mesmas características e atributos são considerados o mesmo.

No modelo de domínio quando um grupo de objectos são tratados como uma unidade, no que diz respeito á informação que estes representam, são considerados agregados. Um agregado define uma barreira e tem como raiz uma entidade e é através dessa entidade que os outros elementos do agregado são acedidos. 

As fábricas e os repositórios são usados para gerir o tempo de vida das entidades. As fábricas são usadas na criação e por vezes para abstrair o tipo concreto do objecto criado. Os repositórios são mecanismos que encapsulam a obtenção, alteração e a procura dos dados do sistema.

Na definição do Modelo de Domínio da solução, foram definidos como agregados as entidades Projecto e Pessoa. O objecto de domínio Tarefa também é considerado como entidade porque possui um identificador único no sistema. O objecto de domínio Registo de Horas é definido como _value object_.

##2.3. Base de dados

As aplicações actualmente utilizam, na sua maioria, um sistema de gestão de base de dados (SGBD) baseado no modelo relacional.

Neste modelo de dados são conhecidos dois conceitos, o conceito de entidade e o conceito de relação. Uma entidade é um elemento caracterizado pelos seus dados sendo esses dados armazenados num registo de uma tabela. A relação determina a forma como os registos de diferentes tabelas se relacionam entre si. A forma de ler e escrever dados nestes sistemas é através da linguagem SQL.

Para perceber se a melhor opção era utilizar o modelo clássico para a persistência dos dados foram analisadas alternativas 
que surgiram com o movimento NoSQL (Not Only SQL).

### NoSQL

As bases de dados NoSQL diferem do modelo relacional porque são tipicamente desenhadas para escalar horizontalmente e podem ter as seguintes características:

 * Não têm _schema_ fixo;
 * Não suportam operações de _join_;
 * Suportam o conceito de BASE;
 * Suportam o conceito de CAP;  

Embora algumas destas características possam parecer negativas, existem vantagens na utilização deste tipo de base de dados dependendo da sua categoria. Em NoSQL as bases de dados podem ser categorizadas como _key-value stores_, base de dados de documentos, base de dados de grafos ou implementações _BigTable_.

#### Key-value stores 

A função principal de um _key-value store_ é guardar um valor associado a uma chave. Para essa função é disponibilizada uma variação da seguinte API:

```csharp
void Put(string key, byte[] data);
byte[] Get(string key);
void Remove(string key);
```

O valor guardado é um _blob_. Esta característica torna desnecessária a definição de um _schema_ dando assim total flexibilidade no armazenamento de dados. Devido ao acesso ser feito através de uma chave este tipo de persistência pode facilmente ser optimizada e ter a sua performance melhorada.

 * **Concorrência** - Num key-value store a concorrência apenas se manifesta quando são feitas operações sobre a mesma chave.

 * **Queries** - Uma _query_ é apenas possível fazer com base na chave e como retorno obtêm-se o valor associado. Esta é uma limitação deste tipo de solução mas que se torna uma vantagem em ambientes em que o único tipo de acesso necessário é com base numa chave, como é o case de um sistema de cache.

 * **Transacções** - A garantia de que as escritas são feitas no contexto de uma transacção só é garantida se for escrita apenas uma chave. É possível oferecer essas garantias para múltiplas chaves mas tendo em conta a natureza de um key-value store de permitir que diferentes chaves estejam armazenadas em diferentes máquinas torna o processo de difícil implementação.

 * **_Schema_** - O _schema_ neste tipo de base de dados é simples, a chave é uma string e o valor é um blob. O tipo e a forma como os dados são estruturados é da responsabilidade do utilizador.

 * **Escalabilidade** - Existem duas formas para o fazer sendo que a mais simples seria separar as chaves. Separar chaves implica decidir a regra de separação, que pode separar as chaves com base no seu primeiro caracter e cada caracter é alojado numa máquina diferente, esta forma torna-se uma não opção quando a máquina onde está a chave não está disponível. Para resolver esse problema é usada replicação.

#### Base de dados de documentos

Uma base de dados de documentos é na sua essência um _key-value store_. A diferença é que, numa base de dados de documentos, o _blob_ de informação é persistido de uma forma semiestruturada, em documentos, utilizando um formato que possa ser interpretado pela base de dados como JSON, BSON, XML, etc, permitindo realizar queries sobre essa informação.
___

ALTERAR bullets -> texto corrido
___
 * **Concorrência** - Existem várias abordagens para resolver este problema como a concorrência optimista, pessimista ou _merge_. 
  * Concorrência Optimista: Antes de gravar informação é verificado se o documento foi alterado por outra transacção, sendo a transacção abortada nesse caso;
  * Concorrência Pessimista: Usa locks para impedir várias transacções de modificarem o mesmo documento. Esta abordagem é um problema para a escalabilidade destes sistemas;
  * Concorrência _merge_: Semelhante à concorrência optimista mas em vez de abortar a transacção permite ao utilizador resolver o conflito entre as versões do documento.

 * **Transacções**- Em alguns casos é dada a garantia de que as operações cumprem com a regra ACID (atomicity, consistency, isolation, durability). Algumas implementações optam por não seguir a regra ACID, desprezando algumas propriedades em detrimento de um aumento de rendimento, usando as regras CAP (Consistency, Availability, Partition Tolerance) ou BASE (Basically Available, Soft State, Eventually Consistent).

 * **_Schema_** - Este tipo de base de dados não necessita que lhe seja definido um _schema à priori_ e não têm tabelas, colunas, tuplos ou relações. Uma base de dados orientada a documentos é composta por vários documentos auto-descritivos, ou seja, a informação relativa a um documento está guardada dentro deste. Isso permite que sejam armazenados objectos complexos (i.e. grafos, dicionários, listas, etc) com facilidade. Esta característica implica que, apesar de poderem existir referências entre documentos a base de dados não garante a integridade dessa relação.

 * **_Queries_** - As _queries_ são feitas com base em índices inferidos automaticamente ou definidos explicitamente pelo programador. Quando o índice é definido o SGBD executa-o e prepara os resultados minimizando o esforço computacional necessário para responder a uma _query_. 
    
    A forma de actualização destes índices difere em cada implementação deste tipo de base de dados, podendo ser actualizados quando os documentos associados a estes índices são alterados ou no momento anterior à execução da _query_. No primeiro caso isso significa que podem ser obtidos resultados desactualizados, uma vez que as _queries_ aos índices têm resultados imediatos e a actualização pode não estar concluída. Na segunda abordagem, se existirem muitas alterações para fazer a _query_ pode demorar algum tempo a responder.

 * **Escalabilidade** - Este tipo de base de dados suporta Sharding, ou seja partição horizontal, o que permite separar documentos por vários servidores.

#### Outros

Existem outros tipos de base de dados NoSQL como o BigTable e as bases de dados de grafos que não foram considerados.

### RavenDB

Tendo em conta o apresentado foi escolhido usar neste projecto uma base de dados de documentos. A escolha recaiu sobre o **RavenDB**, uma base de dados de documentos implementada sobre a Framework [.NET](http://www.microsoft.com/net). É uma solução transaccional, que armazena os dados no formato [_JSON_](http://www.json.org/) e para leitura e escrita de dados suporta a utilização da componente [_Linq_](http://msdn.microsoft.com/en-us/library/bb308959.aspx) da framework .Net ou de uma API RESTful disponibilizada através do protocolo HTTP. 

A escolha desta Base de Dados no desenvolvimento deste projecto implicou que fossem feitas alterações à forma como são definidas as relações entre as entidades de domínio. Existe diversas formas de representar essas relações, em RavenDB, tendo sido consideradas as seguintes.

#### Desnormalização

Uma abordagem possível para este problema é a desnormalização dos dados, ou seja, uma entidade agregadora passa a conter parte (ou a totalidade) da informação das entidades que agrega, em vez de ter apenas uma chave estrangeira para estas. Esta técnica permite que, ao carregar uma entidade agregadora tenhamos disponível parte da informação dos seus filhos, bem com a chave estrangeira para os carregar caso seja necessário agir sobre estes.

#### _Includes_

Com a desnormalização surgem problemas com a actualização de dados pelo facto de existir informação fora do documento da entidade a que esta pertence. Actualizar um campo em entidades que são referenciadas por outras tem um custo computacional elevado, para o servidor. 

Para evitar estas situações o RavenDB oferece o método _Include_ que permite carregar entidades, através da sua chave, aquando da execução de uma _query_. Este método carrega a entidade pretendida para a _cache_ o que faz com que acessos a essa entidade não se traduzam em _queries_ ao SGBD.

#### _Live Projections_

Como complemento para a solução anterior o RavenDB oferece forma de, na base de dados, juntar documentos e transformar o resultado em objectos customizados. Esta funcionalidade permite carregar documentos relacionados, escolhendo as propriedades de cada um que se pretende carregar.

Das abordagens apresentadas escolheu usar-se a Desnormalização, conseguindo desta forma ter alguma informação sobre as entidades relacionadas, sem ter que as carregar explicitamente. As alterações ao modelo consistem em guardar nas entidades raiz a referência e parte da informação relevante da entidade que esta referencia, em vez de guardar apenas o seu identificador. 

Na actual fase de desenvolvimento apenas é usado o método de Desnormalização. Este método é usado para relacionar Projectos com Pessoas e Tarefas, Tarefas com Pessoas e Registo de Horas com Pessoas.

#3. Web

Um dos principais objectivos do projecto é que a sua disponibilização seja o mais abrangente possível e para isso é disponibilizado uma aplicação Web e uma API RESTful.

##3.1. Aplicação Web

A aplicação Web tem como finalidade disponibilizar ao utilizador uma interface para aceder aos dados da infra-estrutura através de qualquer user agent. Para a implementação deste componente é usada a framework ASP.NET MVC leccionada no decorrer do curso. Esta framework, como o próprio nome sugere, implementa o padrão model-view-controller(MVC).

Na implementação da componente visual da aplicação Web é usado HTML5 e CSS3 e o aspecto visual é conseguido utilizando os componentes disponibilizados no kit Twitter Bootstrap. As frameworks javascript jQuery e Knockout permitem tornar a interacção com o utilizador mais fluída e interactiva.

##3.2. Web API RESTful

### ReST

ReST (Representational State Transfer) é uma forma de obter informação de uma aplicação Web. Assenta sobre o protocolo HTTP e os métodos HTTP (get, post, put, delete, etc) são usados para identificar a acção a realizar sobre o url. Cada url expõe um recurso disponibilizado pela aplicação Web.

### WebApi

Na implementação da API há a preocupação de que o url de acesso ao recurso seja o mais perceptível por parte do utilizador (e.g. http://host/api/projects/1, http://host/api/projects/1/tasks/3). 

A implementação de uma Api ReST permite tornar acessíveis os recursos da infra-estrutura para que esta informação possa ser acedida por qualquer utilizador. O utilizador pode assim:
 * Consumir a informação 
 * Integrar dois sistemas diferentes, a infra-estrutura implementada neste projecto com uma outra (e.g. uma aplicação Web que possibilite a facturação de serviços pode ser usada para gerar facturas conforme as horas registadas)
 * Organizar a informação de uma forma a que consiga melhor interpreta-la.
 * Disponibilizar a informação num dispositivo móvel.

Devido a estas características optou-se por, em paralelo com a aplicação Web desenvolver uma API ReST. 

#4.    Ferramentas usadas

No desenvolvimento deste trabalho são usadas aplicações e componentes implementadas por terceiros. De forma a facilitar a obtenção dessas componentes e a sua integração no ambiente de desenvolvimento foi usada a extensão **NuGet**, para a aplicação Microsoft Visual Studio 2010. Esta extensão permite fazer a procura e _download_ de componentes, da sua _galeria online_, e faz a gestão de todas as referências usadas no projecto do Visual Studio para essas componentes.

* RavenDB
 
Como foi dito anteriormente, uma das aplicações usadas é o RavenDB. O RavenDB é um sistema de base de dados de documentos implementado sobre a Framework .NET. 

O RavenDB é desenvolvido e distribuido pela [hibernating rhinos](http://hibernatingrhinos.com/). No ambito deste projecto é usado o _SaaS_ (Software as a Service) RavenHQ, disponibilizado pelo AppHarbor em parceria com os criadores do RavenHQ.

* AttributeRouting

Para definir o routing usado na API ReST foi usada a biblioteca AttributeRouting. Esta biblioteca permite configurar o uri de acesso ao recurso através da utilização de atributos. 

* AutoMapper

AutoMapper é uma biblioteca que, com base em configurações predefinidas, possibilita a conversão entre objectos de tipos diferentes.
Tem a funcionalidade de corresponder propriedades do objecto fonte e do objecto destino com base no nome usando o paradigma _convention over configuration_.

* Knockout.js

A framework javascript jQuery disponibiliza uma forma de manipular o DOM e simplificar com o browser. A framework Knockout leva essa interação mais longe e através do padrão model-view-view model (MVVM) permitindo que as interações com o utilizador sejam mais simples e fluidas.

Neste padrão existem três interveniente, a view, o modelo e o view model. É através da view que o utilizador indica a ação a realizar, acção essa que é passada ao view-model. O view model envia e obtem dados do modelo e quando é alterado notifica os elementos da view que estão a observar os seus atributos.

![MVVM](http://www.lucidchart.com/publicSegments/view/4fa547bc-de04-48d0-b663-37860a58bb74/image.png =150x150)

Na framework knockout a view é definida no HTML, utilizando o atributo data-*, o model é representado por um objecto javascript assim como o viewmodel.
