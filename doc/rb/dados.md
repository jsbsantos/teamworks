Dados
=

\label{sec:dados}

Para a realização deste projecto foi usada a base de dados de documentos RavenDB, que é uma base de dados transaccional, *open-source* e implementada sobre a *framework* .NET. Esta base de dados é composta por um servidor e um cliente e os dados são guardados sem *schema* rígido em documentos no formato JSON.

Na figura \ref{fig:interacaoraven} pode observar-se a interacção da plataforma com o cliente RavenDB.

![Interacção da plataforma Teamworks com a base de dados.\label{fig:interacaoraven}](http://www.lucidchart.com/publicSegments/view/4fd76e6a-3ef0-4875-99c1-4ac60a78da40/image.png)

O acesso ao servidor é feito através de uma Api ReSTful e este tem a responsabilidade de persistir os dados.

O cliente expõe as funcionalidades do servidor e permite a interacção através de código .NET . Os dados enviados e recebidos do cliente são instâncias de classes *POCO*s (*Plain Old CLR Object*) o que simplifica a sua utilização.

\lstset{caption={Utilização do cliente RavenDB.},label={exemplocliente}}

````csharp
var person = new Person {
               Email = "johndoe@world.com"
               Username = "johndoe",
               Password = "wh0 am i?"
             }

using(var session = store.OpenSession()) {
  session.Store(person);
  session.SaveChanges();
}

using(var session = store.OpenSession()) {
  var entity = session.Query<Person>()
    .Where(p => p.Username == "johndoe")
    .Single();

  entity.Username = "doejohn";
  session.SaveChanges();
}
```` 

A lista \ref{exemplocliente} demonstra a utilização do cliente. Pode observar-se a utilização de *POCO*s e do padrão *Unit of Work* \cite[pp.~184-194]{patterns} pois todas as alterações feitas ao cliente são persistidas na base de dados numa única transacção quando é chamado o método `SaveChanges`. A variável `store` define a configuração do cliente, a comunicação com o servidor e todos os mecanismos da base de dados.

Modelo de Dados
-

Devido à escolha de uma base de dados de documentos a forma de modelar os dados da aplicação tem de ser ajustada.  
Uma característica a ter em conta neste tipo de base de dados é a impossibilidade de fazer operações de `JOIN`, o que implica que um documento deve guardar toda a informação necessária da entidade que representa.

A abordagem aconselhada para a modelação de dados neste tipo de base de dados é o uso do padrão *Aggregate* \cite[pp.~126-127]{domaindrivendesign} para a escolha de que informação fica em cada documento. O padrão define um agregado como um grupo de objectos tratados como um só, tendo em conta alterações no seu conteúdo. 
As referências externas estão limitadas à raiz do agregado, que controla todas as alterações aos objectos contidos nos seus limites.
Como na definição do modelo de domínio foi usada uma abordagem *Domain Driven Design* os agregados identificados são guardados como documentos independentes.
A única excepção a esta regra é a entidade actividades que tem um documento próprio. Esta decisão deve-se à possibilidade de um projecto ter várias actividades associadas e para evitar que o documento atinja dimensões elevadas.

A figura \ref{fig:diagramadeclassesmodelo} representa o modelo de dados da solução, através de um diagrama de classes UML.

![Diagrama UML de classes do modelo de dados.\label{fig:diagramadeclassesmodelo}](http://www.lucidchart.com/publicSegments/view/4fdbbe6c-4818-4978-a979-22210a490e1b/image.png)
 
A autorização na plataforma utiliza um *bundle*[^bundle] do RavenDB. O *bundle* permite fazer a gestão de obtenção, alteração e remoção de documentos baseado no utilizador. Este *bundle* define quatro intervenientes no processo de autorização: o utilizador (`AuthorizationUser`), o *role* (`AuthorizationRole`), a operação que o utilizador pode fazer (`OperationPermission`) e a permissão necessária para aceder ao documento (`DocumentPermission`). 

![Diagrama UML de autorização.\label{autorizacao}](http://www.lucidchart.com/publicSegments/view/4fd9c8d1-77b0-457e-8520-39800adcb320/image.png)

Para um utilizador aceder a um documento tem de lhe estar associado, pertencer a um *role* associado ao documento, ter a operação exigida pelo documento ou pertencer a um *role* que a tenha. No entanto este módulo permite que seja usado um tipo personalizado para representação do utilizador desde que defina as mesmas propriedades que a implementação disponibilizada pelo Raven (`AuthorizationUser`). No modelo de dados da plataforma o utilizador é substituído por `Person` e o *role* não é utilizado.

[^bundle]: Módulo de extensão das funcionalidades da base de dados - Ver anexo \ref{app:ravendb-bundle}.
