Aplicação Web
=

A aplicação web disponibiliza uma interface que possibilita que o utilizador interaja com a infra-estrutura implementada usando a *framework* ASP.NET MVC \cite{aspnetmvc}, juntamente com outras ferramentas que serão discutidas neste capítulo.

Numa primeira fase decidiu-se que a aplicação web seria apenas um consumidor da Api web e seria através dela que obteria toda a informação que necessitava. Com o decorrer do projecto concluiu-se que os dados necessários para a aplicação web não eram possíveis de obter através da web Api sem desvirtuar o seu objectivo de expor os objectos de domínio como recursos.

À semelhança da Api foi definido um `Controller` base com a propriedade `DBSession`, afectada antes de ser chamada a *action*.
A persistencia dos dados é feita depois de ser executado o resultado da chamada à *action*


Componente Visual
-

Para implementação da componente visual da aplicação web são usadas as tecnologias HTML5, CSS3, o *kit* Twitter Bootstrap \cite{bootstrap} e knockout.js \cite{knockout}.

\label{sec:Knockout}

A framework javascript Knockout, foi usada a para tornar as páginas web mais dinâmicas e melhorar a experiência do utilizador. Esta *framework* implementa o padrão MVVM (**M**odel-**V**iew-**V**iew**M**odel) e faz a actualização dos elementos HTML da página, de acordo com as alterações ao modelo (*view model*) que lhe está associado.

Este comportamento é conseguido definindo em javascript um *view model*, que é um objecto javascript que contém propriedades observáveis (observables) por elementos HTML.
A definição de uma associação entre um elemento HTML e uma propriedade do *view model*, denominada *binding*, permite que seja feita a actualização do elemento com o valor da propriedade que lhe está associada e vice-versa. É através dos atributos definidos como *observables* (tipo introduzido pela *framework knockout*) dos *view model* que é feita a actualização da informação presente na página, através de pedidos AJAX \cite{ajax}, respondendo a interacções do utilizador. 
É possível que existam vários elementos HTML a observar a mesma propriedade do *view model*, tomando todos o mesmo valor quando esta é alterada. Para extender as funcionalidades da *framework* foram criadas novos *bindings* e novos *extensões*.

***Bindings***

Um *binding* é composto por duas funções, *read* e *write*, chamadas quando é feita umaalteração ao atributo do *view model* associado. Quando o valor do *observable* é alterado a função *write* é chamada e para obter o valor do *observable* é chamada a função *read*. 

Para utilizar a componente *typeahead* do *kit* visual foi definido o *binding* *typeahead*. Este *binding* inicia no elemento html a que está associado a componente *typeahead* e se ao binding estiver associada uma função esta é invocada com o *typeahead* criada para que este possa ser configurado.

Assim como o *binding typeahead*, os *bindings datepicker* e *timeago* iniciam componentes de soluções *open source* incluídas na infraestrutura. 
O *datepicker* como o próprio nome indica fornece uma forma visual de escolher uma data que quando a data é alterada altera o valor do elemento html a que o binding está associado. 
O *binding timeago* altera valor do elemento html de uma data para o tempo relativo dessa data para com a data actual.
 
**Extensões**

É possível ainda alterar o comportamento por omissão que estas propriedades observáveis têm através de extensões. As extensões tiram partido das propriedades observáveis serem definidas como uma função, acrescentando-lhes novas propriedades.

Uma das extensões criadas está relacionada forma como é apresentada a data, usando a função toString() do objecto Date do javascript, e é aplicada a todas as propriedades do *view model* que representam datas. A extensão *isoDate* cria a propriedade `formatted` na propriedade observável do *view model*, que apresenta a data no formato "dd/mm/yyyy".

O valor registado na propriedade de "Duração" dos registos de horas é persistido em segundos, tornando-se pouco prático fazer a sua apresentação nesse formato.
Para apresentar ao utilizador esse valor num formato mais perceptivel foi criada a extensão `duration` que transforma essa duração numa string do formato "5h", "5h 20m" ou "20min". Além disso esta extensão permite fazer a conversão inversa, transformando uma string, num dos formatos enumerados anteriormente, no seu correspondente em segundos, facilitando a introdução do tempo dispendido numa actividade, no registo de horas do utilizador.

Com a associação de propriedades observáveis definidas no *view model* a elementos HTML pertencentes a formulários, surgiu a necessidade de marcar alguns campos como obrigatórios, impedindo a submissão do formulário quando estes não estão preenchidos. Para conseguir este comportamento definiu-se a extensão `required` que suporta a definição de uma mensagem que será mostrada quando a propriedade não se encontrar preenchida.

*Action Filters*
-

A framework ASP.NET MVC suporta a definição de *action filters* através de atributos. Estes *action filters* são usados para modificar o comportamento das *actions* e podem alterar o estado do contexto HTTP associado ao pedido.

\label{sec:AjaxOnlyAttribute}
O atributo AjaxOnlyAttribute é usado para garantir que o pedido foi feito através de AJAX. Caso contrário é lançada uma excepção, terminando a execução do pedido com uma mensagem de erro indicando que apenas pedidos HTTP são suportados.

\label{sec:NoDbExceptionAttribute}
Uma vez que a base dados usada é o RavenDB, e estando a base de dados alojada num servidor diferente do da aplicação web, é possível que existam falhas na comunicação. Para evitar que o utilizador veja a página de erro enviada pelo servidor IIS por omissão, foi criado um atributo que filtra o resultado do processamento de um pedido HTTP. Quando o pedido termina com uma excepção do tipo *SocketException* o conteúdo da resposta é alterado e é mostrada uma *view* que alerta o utilizador para a indisponibilidade da aplicação. 

\label{sec:FormsAuthenticationAttribute}
Durante o processamento dos pedidos é muitas vezes necessário saber qual o utilizador que lhes está associado. O atributo *FormsAuthenticationAttribute*  coloca na propriedade User do contexto HTTP a instância do tipo *Person* que representa o utilizador, obtida através do *cookie* de sessão do utilizador, criado quando este entra na aplicação.

\label{sec:SecureAttribute}
A autorização de utilizadores para aceder aos recursos é feita com recurso ao *bundle* de autorização. Este *bundle* filtra os pedidos de alteração ou obtenção de documentos com base nas permissões associadas à sessão *DocumentSession* sobre a qual são executados. O atributo *SecureAttribute* é usado para associar à sessão as permissões do utilizador que fez o pedido, garantindo que todas as interacções com a base de dados são feitas com essas permissões. Este comportamento é conseguido porque o tipo *DocumentSession* utiliza o padrão *Unit of Work*.

\label{sec:SecureProjectAttribute}
A utilização de *routes* explicitas, onde cada pedido identifica univocamente todas as entidades associadas ao recurso que se pretende aceder, obriga a que exista em todos os pedidos o parâmetro correspondente ao identificador do projecto a que o recurso pertence. Para impedir que utilizadores acedam a recursos para os quais não têm acesso, no atributo SecureProjectAttribute é verificado se o utilizador pode aceder ao projecto cujo identificador é passado no pedido. Caso não tenha acesso é retornado um erro "404 - Not Found".

Autenticação
-

A autenticação na aplicação web é feita através de um formulário onde o utilizador insere o nome de utilizador e a *password*. A validação dos dados inseridos é feita de forma semelhante à utilizada na Api, com a diferença que na aplicação web é usada uma cookie para manter o utilizador autenticado nos pedidos subsequentes. Para isso são usadas funcionalidades do modo de autenticação *forms* da *framework* ASP.NET. 

As classes que expõem as funcionalidades necessárias são a classe `FormsAuthentication`, usada para gerir o *cookie* de autenticação[^cookie], e o módulo `FormsAuthenticationModule` para manter o utilizador autenticado. 

[^cookie]: A cookie usada tem o nome `.tw_auth`

Além da autenticação por utilizador e password, é possível que o utilizador faça o registo e autenticação através da utilização de um provider OpenID \cite{OpenID}, sendo que apenas é suportado o provider OpenID da Google.

Depois de validar os dados do utilizador com sucesso a cookie é colocada na resposta usando a classe `FormsAuthentication`.
A cookie tem como valor o identificador da instância de *Person* que foi obtida da base de dados no processo de autenticação. 

O utilizador mantém a identidade dos pedidos anteriores pois o módulo `FormsAuthenticationModule`, em cada pedido, coloca o valor da *cookie* na propriedade `User` do pedido. A acção do módulo é complementada pelo filtro `FormsAuthenticationAttribute` que substitui o `IIdentity` do pedido por um `PersonIdentity` que disponibiliza a entidade `Person` do utilizador autenticado ao código executado nas *actions*.


Ferramentas externas
- 

No desenvolvimento da aplicação foram usadas algumas *frameworks* e bibliotecas externas para nos auxiliar na resolução de problemas e aumentar a qualidade da solução implementada. 

\label{sec:Attribute Routing}

Uma das frameworks usadas é o *Attribute Routing* \cite{attributerouting} que permite definir *routes* ASP.NET MVC através de atributos, disponibilizando atributos que representam os métodos HTTP (GET, POST, PUT e DELETE) podem ser apliados em *controllers* ou *actions*. Com a utilização desta biblioteca conseguiram criar-se *routes* intuitivas e de facil reconhecimento para o utilizador, em que cada uma identifica univocamente todas as entidades associadas ao recurso que representa.

\label{sec:Automapper}

Com a utilização de objectos como parâmetros das *actions* advém a necessidade de diferenciar esses parâmetros dos tipos usados para representar as entidades de dominio. Para facilitar a conversão entre estes tipos é usada a biblioteca Automapper que permite configurar a forma como é feita essa conversão, evitando que seja repetido o código de conversão entre objectos.

Devido à complexidade da informação apresentada nas *views* foram definidos tipos que a representam, designados de *view models*. A conversão entre entidades de dominio e os *view model* é feita com recurso à biblioteca Automapper.

Esta abordagem foi usada também na web API, existindo *view models* que representam os parâmetros recebidos e os resultados retornados pelas suas *actions* .
