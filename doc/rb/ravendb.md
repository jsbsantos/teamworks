RavenDB
=

O RavenDB [#ravendb]() é uma base de dados de documentos implementada sobre a *framework* .NET [#net]() que suporta a componente Linq [#linq]() para *querying*. 
A base de dados é dividida em dois blocos o servidor e o cliente. Na figura [blocosravendb]() observa-se a interação da infra-estrutura com o RavenDB, utilizando o cliente. 

![Interacção da infra-estrutura Teamworks com a base de dados RavenDB\label{blocosravendb}](http://www.lucidchart.com/publicSegments/view/4fd76e6a-3ef0-4875-99c1-4ac60a78da40/image.png)

O servidor é transaccional, armazena os dados no formato JSON [#json]() e tem como interface um serviço web disponibilizado através do protocolo HTTP. 

O cliente tem como objectivo expor todas as funcionalidades do servidor através de uma api. 

Cliente RavenDB
-

Para interacção com o cliente são usadas classes POCO o que torna desnecessária a utilização de um ORM ou qualquer sistema de correspondência entre objectos de domínio e os objectos persistidos. O cliente para além de gerir a comunicação com o servidor o cliente é responsável por fazer cache dos pedidos feitos e pela implementação do padrão *Unit of Work*[#unitofwork]().

A infra-estrutura utiliza principalmente duas classes da api cliente, *IDocumentStore* e *IDocumentSession*.
A classe *IDocumentSession* representa uma sessão e permite obter dados, persistir dados e apagar dados da base de dados. O padrão *Unit of Work* é implementado nas instâmcias desta classe e é dada a garantia que todas as alterações serão persistidas numa única transacção.
A classe *IDocumentStore* é uma fabrica para a criação de sessões.

Indices
-

Relações entre documentos
-

As relações entre documentos, devido à inexistência de operações *JOIN*, podem ser representadas de várias formas. As formas consideradas são a utilização do identificador para incluir as entidades no pedido da entidade principal, a desnormalização e as *live projections*.

### Inclusão no pedido

Nesta opção cada documento guarda o identificador do(s) documento(s) com que está relacionado (e.g. os documentos que representam projectos guardam o identificador dos documentos que representam tarefas) e quando é obtido um documento principal são também obtidos todos os documentos que lhe estão associadas. 

No RavenDB esta opção é suportada pelo método *Include* que recebe os identificadores das entidades a carregar em paralelo com as entidades principais. As entidades incluídas são adicionadas à sessão pelo cliente RavenDB. 

### Desnormalização

A desnormalização de um documento consiste em extrair as informações relevantes, de outros documentos, e replicá-las no documento a persistir.

A figura [desnormalizacao](#) mostra um exemplo de desnormalização. O documento Projecto guarda para além do identificar dos utilizadores que lhe estão associados o seu Nome. Numa situação em que seja necessário apenas o Nome dos utilizadores associados essa informação já está no documento.

![Exemplo de desnormalização de informação\label{desnormalizacao}](http://www.lucidchart.com/publicSegments/view/4fd722d2-6770-4fe6-951d-51600a5705ae/image.png)

Esta situação tem a vantagem de reduzir o número de pedidos à base de dados porque o documento guarda toda a informação necessária.
Esta abordagem tem a desvantagem de que qualquer alteração a um documento desnormalizado implica a alteração de todos os documentos que o utilizam.

### Live Projections

O *RavenDB* oferece ainda forma de juntar e transformar documentos no servidor obtendo como resultado objectos diferentes dos objectos persistidos. Esta funcionalidade permite carregar documentos relacionados, escolhendo as propriedades de cada um que se pretende.

Na implementação deste projecto optou-se por estabelecer relações entre documentos através do seu identificador, tirando partido da funcionalidade *Include* oferecida pelo cliente RavenDB.

Bundles
-

No caso de as funcionalidades disponibilizadas pelo servidor RavenDB não serem suficientes existem Bundles que extendem as funcionalidades oferecidas. 
Os Bundles oferecidos com a build do RavenDB são:

 * **Sharding and Replication**.
 * **Quotas**, coloca limites ao tamanho na base de dados.
 * **Expiration**, remove documentos expirados automáticamente.
 * **Index Replication**, replica indices RavenDB para base de dados SQL Server.
 * **Authentication**, autentica utilizadores na base de dados usando OAuth.
 * **Authorization**, permite a gestão de grupos, *roles* e permissões.
 * **Versioning**, automaticamente gera versões dos documentos quando são alterados ou removidos.
 * **Cascade Deletes**, automatiza operações de remoção *cascade*.
 * **More Like This**, retorna documentos relacionados com o documento indicado.
 * **Unique Constraints**, adiciona a possibilidade de definir *unique constraints* em documentos RavenDB.