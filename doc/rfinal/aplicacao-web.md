Aplicação Web
=

A aplicação web disponibiliza uma interface que possibilita que o utilizador interaja com a infra-estrutura implementada usando a *framework* ASP.NET MVC \cite{aspnetmvc}, juntamente com outras ferramentas que serão discutidas neste capítulo.
Numa primeira fase foi decidido que a aplicação Web era apenas um consumidor da Api obtendo toda a informação que necessitava apresentar ao utilizador da Api.
Com o decorrer do projecto concluiu-se que os dados necessários para a aplicação Web não eram possíveis de obter através da web Api sem desvirtuar o seu objectivo de expor os objectos de domínio como recursos.

Desta forma alterou-se o funcionamento da aplicação web para que o seu funcionamento fosse independente da web Api, conseguindo-se uma maior qualidade na informação apresentada na aplicação web e adequaram-se as respostas da web Api, enviando a informação necessária para representar o recurso com o qual é feita a interacção.

Componente Visual
-

Para a realização da componente visual da aplicação web são usadas as tecnologias HTML5, CSS3, o *kit* Twitter Bootstrap \cite{bootstrap} e knockout.JS \cite{knockout}.

\label{sec:Knockout}

A framework javascript Knockout, foi usada a para tornar as páginas web mais dinâmicas e melhorar a experiência do utilizador. Esta framework implementa o padrão MVVM (**M**odel-**V**iew-**V**iew**M**odel) e faz a actualização dos elementos HTML da página, de acordo com as alterações ao modelo (*view model*) que lhe está associado.

Este comportamento é conseguido definindo em javascript um *View Model*, que é um objecto javascript, que contém propriedades observáveis (observables) por elementos HTML. A definição de uma associação entre um elemento HTML e uma propriedade do *view model*, denominada *binding*, permite que seja feita a actualização do elemento com o valor da propriedade que lhe está associada e vice-versa. É possível que existam vários elementos HTML a observar a mesma propriedade do *view model*, tomando todos o mesmo valor quando esta é alterada. É através destes *view model* que é feita a actualização da informação presente na página, através de pedidos AJAX \cite{ajax}, respondendo a interacções do utilizador.

** Extensões **

É possível ainda estender o comportamento que estas propriedades observáveis têm por omissão definindo-lhes extensões. As extensões tiram partido das propriedades observáveis serem definidas como uma função, acrescentando-lhes novas propriedades.

*isoDate*

Uma das extensões criadas está relacionada forma como é apresentada a data, usando a função toString() do objecto Date do javascript, e é aplicada a todas as propriedades do *view model* que representam datas. Esta extensão cria a propriedade "*formatted*" na propriedade observável do *view model* que mostra a data no formato dd/mm/yyyy.

*Duration*

Como forma de converter para um formato mais agradável para o utilizador o tempo introduzido nos registos de horas, que é persistido em milissegundos, foi criada uma extensão. Esta extensão transforma uma string, de um formato específico, no seu valor correspondente em milissegundos, e vice-versa. As string aceites nesta extensão são do formato "5h", "5h 20m" ou "20min".

*Required*

Com a associação se propriedades observáveis, definidas no *view model*, a elementos HTML pertencentes a formulários, surgiu a necessidade de marcar alguns campos como obrigatórios, impedindo a submissão do formulário quando estes não estão preenchidos. Para obter este comportamento definiu-se a extensão *required*. Esta extensão permite a definição de uma mensagem, que será mostrada quando a propriedade não se encontrar preenchida.

** Bindings **
 
typeahead

highlight

datepicker

timeago

*Action Filters*
-

A framework ASP.NET MVC suporta a definição de *action filters*, através de atributos. Estes *action filters* são usados para modificar o comportamento das *actions*, podendo alterar o estado do contexto HTTP associado ao pedido.

\label{sec:AjaxOnlyAttribute}
O atributo AjaxOnlyAttribute é usado para garantir que o pedido foi feito através de AJAX. Caso contrário é lançada uma excepção, terminando a execução do pedido com uma mensagem de erro indicando que apenas pedidos HTTP são suportados.

\label{sec:NoDbExceptionAttribute}
Uma vez que a base dados usada é o RavenDB, e estando a base de dados alojada num servidor diferente do da aplicação web, é possível que existam falhas de comunicação com esta. Para evitar que o utilizador veja a página de erro, enviada pelo servidor IIS por omissão, foi criado um atributo que filtra o resultado do processamento de um pedido HTTP. Quando o pedida termina com uma excepção do tipo *SocketException* o conteúdo da resposta é alterado e em vez da página de erro por omissão é mostrado o resultado de uma *view* alertando o utilizador para a indisponibilidade da aplicação. 

\label{sec:FormsAuthenticationAttribute}
Durante o processamento dos pedidos, é muitas vezes necessário saber qual o utilizador que o fez. O atributo FormsAuthenticationAttribute usa o *cookie* de autenticação do utilizador que enviou o pedido, que é criado quando é feita a autenticação do utilizador, e coloca na propriedade User, do contexto HTTP, a instância do tipo Person que representa o utilizador.

\label{sec:SecureAttribute}
A autorização de utilizadores para aceder aos recursos é feita com recurso ao *bundle* de autorização. Este *bundle* filtra os pedidos de alteração ou obtenção de documentos, com base nas permissões associadas à sessão DocumentSession sobre a qual são executados. O atributo SecureAttribute é usado para associar à sessão as permissões do utilizador que fez o pedido garantindo que, graças à utilização do padrão *Unit of Work*, todas as interacções com o RavenDB são feitas com essas permissões.

\label{sec:SecureProjectAttribute}
A utilização de *routes* explicitas, onde cada pedido identifica univocamente todas as entidades associadas ao recurso que se pretende aceder, obriga a que exista em todos os pedidos o parâmetro correspondente ao identificador do projecto a que o recurso pertence. Para impedir que utilizadores acedam a recursos para os quais não tenham acesso, no atributo SecureProjectAttribute é verificado se o utilizador em acesso ao projecto cujo identificador é passado no pedido. Caso não tenha acesso é retornado um erro "404 - Not Found".

Autenticação
-

A autenticação na aplicação web é feita através de um formulário onde o utilizador insere o nome de utilizador e a *password*. A validação dos dados inseridos é feita de forma semelhante à utilizada na Api, com a diferença que na aplicação web é usada uma cookie para manter o utilizador autenticado nos pedidos subsequentes. Para isso são usadas funcionalidades do modo de autenticação *forms* da *framework* ASP.NET. 

As classes que expõem as funcionalidades necessárias são a classe *FormsAuthentication*, usada para gerir o *cookie* de autenticação[^cookie], e o módulo *FormsAuthenticationModule* para manter o utilizador autenticado. 

[^cookie]: A cookie usada tem o nome *.tw_auth*

Além da autenticação por utilizador e password, é possível que o utilizador faça o registo e autenticação através da utilização de um provider OpenID \cite{OpenID}, sendo que apenas é suportado o provider OpenID da Google.

Depois de validar os dados do utilizador com sucesso a cookie é colocada na resposta usando a classe *FormsAuthentication*.
A cookie tem como valor o identificador da instância de *Person* que foi obtida da base de dados no processo de autenticação. 

O utilizador mantém a identidade dos pedidos anteriores pois o módulo *FormsAuthenticationModule*, em cada pedido, coloca o valor da *cookie* na propriedade *User* do pedido. A acção do módulo é complementada pelo filtro *FormsAuthenticationAttribute* que substitui o *IIdentity* do pedido por um *PersonIdentity* que disponibiliza a entidade *Person* do utilizador autenticado ao código executado nas *actions*.


Ferramentas externas
- 

No desenvolvimento da aplicação foram usadas algumas frameworks e bibliotecas externas, para nos auxiliar na resolução de problemas e melhorar a qualidade da solução implementada. 

\label{sec:Attribute Routing}

Uma das bibliotecas usadas é *Attribute Routing* \cite{attributerouting}, que permite definir *routes* através de atributos, disponibilizando atributos que representam os métodos HTTP (GET, POST, PUT e DELETE) podem ser apliados em *controllers* ou *actions*. Com a utilização desta biblioteca conseguiram criar-se *routes* intuitivas e de facil leitura e reconhecimento para o utilizador.

\label{sec:Automapper}



**View Models**
