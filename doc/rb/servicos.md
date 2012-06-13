Serviços
=

Repositório
-

<span style="background-color:yellow">Descrição do padrão Repository</span>

Tendo em conta as caracteristicas do cliente RavenDB enunciadas optou-se por não criar uma camada de abstracção de acesso a dados. Para esta decisão contribuiu o facto de toda a infra-estrutura estar fortemente ligada ao RavenDB e a alteração deste SGBD para outro implicar alteração ao modelo de domínio.

RavenDB
-

Um dos serviços disponibilizados é o cliente RavenDB para acesso aos dados da infra-estrutura, a inicialização do cliente é feita na camada de serviços assim como toda a gestão das sessões associadas ao cliente.
Para isso é disponibilizada a propriedade `Global.Raven.CurrentSession`

`IDocumentSession CurrentSession { get; }

````

A sessão tem o tempo de vida de um pedido HTTP, é criada durante o processamento do pedido e no final do pedido as alterações feitas à Sessão são persistidas na base de dados.
A persistência é feita depois da *action* correspondente ser executada pela classe *RavenHandler* registada no *pipeline* HTTP durante o arranque da api. Esta classe verifica se o pedido foi realizado com sucesso e persiste as alterações usando o código seguinte.