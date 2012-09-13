Aplicação Web
=

A aplicação web disponibiliza uma interface que possibilita ao utilizador interagir com a infra-estrutura, que é implementada usando a *framework* ASP.NET MVC \cite{aspnetmvc} juntamente com outras ferramentas que serão enunciadas neste capítulo.

Numa primeira fase decidiu-se que a aplicação web seria apenas um consumidor da Api e seria através desta que obteria toda a informação que necessitava. Com o decorrer do projecto concluiu-se que os dados necessários para a aplicação web não eram possíveis de obter através da Api sem desvirtuar o seu objectivo de expor os objectos de domínio como recursos.

À semelhança da Api foi definido um `Controller` base com a propriedade `DBSession`, afectada antes de ser chamada a *action*. Após a execução do resultado da *action* são persistidas as alterações feitas à sessão.


Componente Visual
-

Para implementação da componente visual da aplicação web são usadas as tecnologias HTML5, CSS3, o *kit* Twitter Bootstrap \cite{bootstrap} e knockout.js \cite{knockout}.

\label{sec:Knockout}

A framework javascript Knockout, foi usada para tornar as páginas web mais dinâmicas e melhorar a experiência do utilizador. Esta *framework* implementa o padrão MVVM (**M**odel-**V**iew-**V**iew**M**odel) e faz a actualização dos elementos HTML da página, de acordo com as alterações ao modelo (*view model*) que lhe está associado.

Este comportamento é conseguido definindo em javascript um *view model*, que é um objecto javascript que contém propriedades observáveis (observables) por elementos HTML.
A definição de uma associação entre um elemento HTML e uma propriedade do *view model*, denominada *binding*. Esta associação permite actualizar o elemento com o valor da propriedade que lhe está associada, e vice-versa. 
É possível que existam vários elementos HTML a observar a mesma propriedade do *view model*, tomando todos o mesmo valor quando esta é alterada. 

Para estender as funcionalidades da *framework* foram criadas novos *bindings* e novas *extensões*.

***Bindings***

Um *binding* é composto pelas funções *read* e *write*, chamadas quando é feita uma alteração ao atributo do *view model* que está associado ao *binding*. Quando o valor do *observable* é alterado a função *write* é chamada e para obter o valor do *observable* é chamada a função *read*. 

Para utilizar a componente *typeahead* do *kit* Twitter Bootstrap foi definido o *binding* *typeahead*. Este *binding* inicia no elemento html a componente *typeahead*. Se ao *binding* estiver associada uma função esta é invocada recebendo como parâmetro o *typeahead* criado para que este possa ser configurado.

Assim como o *binding typeahead*, os *bindings datepicker* e *timeago* iniciam componentes de soluções *open source* incluídas na infra-estrutura. 
O *datepicker*, como o próprio nome indica, fornece uma forma visual de escolher uma data. Quando a data é alterada o valor do elemento html a que o binding está associado é alterado. 
O *binding timeago* altera valor do elemento html de uma data, transformando-o na diferença relativamente à data actual.
 
**Extensões**

É possível alterar o comportamento por omissão que estas propriedades observáveis têm através de extensões. As extensões tiram partido das propriedades observáveis serem definidas como uma função, acrescentando-lhes novas funcionalidades.

Uma das extensões criadas está relacionada com a forma como é apresentada a data e é aplicada a todas as propriedades do *view model* que representam datas. Essa extensão tem o nome *isoDate* e cria a propriedade `formatted` na propriedade observável do *view model*, que apresenta a data no formato "dd/mm/yyyy".

O valor registado na propriedade de duração dos registos de horas é persistido em segundos. Para apresentar ao utilizador esse valor num formato mais perceptível foi criada a extensão `duration` que transforma essa duração numa string do formato "5h", "5h 20m" ou "20min". Além disso esta extensão permite fazer a conversão inversa, transformando um desses formatos em segundos. Esta funcionalidade facilita a introdução do tempo despendido numa actividade, no registo de horas do utilizador.

Com a associação de propriedades observáveis definidas no *view model* a elementos HTML pertencentes a formulários, surgiu a necessidade de marcar alguns campos como obrigatórios, impedindo a submissão do formulário quando estes não estão preenchidos. Para conseguir este comportamento definiu-se a extensão `required` que suporta a definição de uma mensagem que será mostrada quando a propriedade não se encontrar preenchida.

*Action Filters*
-

A *framework ASP.NET MVC* suporta a definição de *action filters*, usados para complementar o comportamento das *actions*. A definição de *action filters* na solução desenvolvida foi feita usando atributos.

\label{sec:AjaxOnlyAttribute}
O atributo `AjaxOnlyAttribute` é usado para garantir que o pedido foi feito através de AJAX. Caso contrário é lançada uma excepção, terminando a execução do pedido com uma mensagem de erro indicando que apenas pedidos AJAX são suportados.

\label{sec:NoDbExceptionAttribute}
Uma vez que a base dados usada é o RavenDB, e estando a base de dados alojada num servidor diferente do da aplicação web, é possível que existam falhas na comunicação. Para evitar que o utilizador veja a página de erro enviada pelo servidor IIS por omissão, foi criado um atributo que filtra o resultado do processamento de um pedido HTTP. Quando o pedido termina com uma excepção do tipo *SocketException* o conteúdo da resposta é alterado e é mostrada uma *view* que alerta o utilizador para a indisponibilidade da aplicação. 

\label{sec:FormsAuthenticationAttribute}
O atributo *FormsAuthenticationAttribute* coloca na propriedade `User` do contexto HTTP a instância do tipo *Person* que representa o utilizador, obtida através do *cookie* de sessão do utilizador, criado quando este se autentica na aplicação.

\label{sec:SecureAttribute}
A autorização de utilizadores para aceder aos recursos é feita com utilizando o *bundle* de autorização. Este *bundle* filtra os pedidos de alteração ou obtenção de documentos com base nas permissões associadas à sessão `DocumentSession` sobre a qual são executados. 

O atributo `SecureAttribute` é usado para indicar à sessão a operação que o utilizador deseja fazer.

\label{sec:SecureProjectAttribute}
A utilização de *routes* explicitas, onde cada pedido identifica univocamente todas as entidades associadas ao recurso que se pretende aceder, obriga a que exista em todos os pedidos o parâmetro correspondente ao identificador do projecto a que o recurso pertence. Para impedir que utilizadores acedam a recursos para os quais não têm acesso, no atributo `SecureProjectAttribute` é verificado se o utilizador pode aceder ao projecto cujo identificador é passado no pedido. Caso não tenha acesso é retornado um erro `404 - Not Found`.

Autenticação
-

A autenticação na aplicação web é feita através de um formulário onde o utilizador insere o nome de utilizador e a *password*. A validação dos dados inseridos é feita de forma semelhante à utilizada na Api, com a diferença que na aplicação web é usada uma *cookie* para manter o utilizador autenticado nos pedidos subsequentes. Para isso são usadas funcionalidades do modo de autenticação *forms* da *framework* ASP.NET. 

As classes que expõem as funcionalidades necessárias são a classe `FormsAuthentication`, usada para gerir o *cookie* de autenticação[^cookie], e o módulo `FormsAuthenticationModule` para manter o utilizador autenticado. 

[^cookie]: A *cookie* usada tem o nome `.tw_auth`

Além da autenticação por utilizador e password, é possível que o utilizador faça o registo e autenticação através da utilização de um *provider OpenID* \cite{openid}, sendo que apenas é suportado o provider *OpenID* da *Google*.

Depois de validar os dados do utilizador com sucesso a *cookie* é colocada na resposta usando a classe `FormsAuthentication`.
A *cookie* tem como valor o identificador da instância de *Person* que foi obtida da base de dados no processo de autenticação. 

O utilizador mantém a identidade dos pedidos anteriores pois o módulo `FormsAuthenticationModule`, em cada pedido, coloca o valor da *cookie* na propriedade `User` do pedido. A acção do módulo é complementada pelo filtro `FormsAuthenticationAttribute` que substitui o `IIdentity` do pedido por um `PersonIdentity` que disponibiliza a entidade `Person` do utilizador autenticado ao código executado nas *actions*.


Ferramentas externas
- 

No desenvolvimento da aplicação foram usadas algumas *frameworks* e bibliotecas externas para auxiliar na resolução de problemas e aumentar a qualidade da solução implementada. 

\label{sec:Attribute Routing}

Uma das frameworks usadas é o *Attribute Routing* \cite{attributerouting} que permite definir *routes* ASP.NET MVC através de atributos. Esta framework disponibiliza atributos que representam os métodos HTTP (GET, POST, PUT e DELETE) que podem ser aplicados em *controllers* ou *actions*. Com a utilização desta framework a definição de *routes* é feita localmente, no controller, e de forma explícita.

\label{sec:Automapper}

Com a utilização de objectos como parâmetros das *actions* advém a necessidade de diferenciar esses parâmetros dos tipos usados para representar as entidades de domínio. Para converter as entidades de domínio em objectos da componente visual ,e vice-versa, é usada a biblioteca Automapper. Com a biblioteca Automapper é possível configurar como é feita a conversão entre objectos.

Para produzir o resultado das *views* foram criados tipos que representam a informação necessária ao efeito. 

A Api também utiliza esta biblioteca para converter as entidades de domínio nos tipos de retorno das *actions*.
