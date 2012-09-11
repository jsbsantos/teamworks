Serviços
=

\label{sec:servicos}

Os serviços disponibilizados pela infra-estrutura encontram-se no *namespace* `Teamworks.Core` e incluem serviços de acesso a dados e autenticação.

Acesso a Dados
-

\label{acesso-a-dados}

É comum para acesso a dados a utilização do padrão *Repository* \cite[pp.~322-327]{patterns}. Este padrão encapsula a lógica de persistência dos objectos de domínio de forma a desacoplar o modelo de domínio da lógica de acesso a dados. Na solução *Teamworks* a implementação do padrão *Repository* é feita pelo cliente *RavenDB*

A inicialização e configuração do cliente (`IDocumentStore`) está presente na propriedade `Database` da classe `Global` e a sua inicialização deve ser feita no início da aplicação.


Segurança
- 

\label{seguranca}

A plataforma impõe as políticas de acesso em conjunto com o *Authorization bundle*. A autorização é feita quando se tenta aceder a um documento. Se um utilizador tentar aceder a um documento e não tiver permissões para o fazer é lançada uma excepção.

A configuração do cliente, para que este valide se é possível interagir com o documento, é feita utilizando métodos de extensão presentes no `assembly` `Raven.Client.Authorization.dll`.

OpenID
-

\label{sec:OpenID}

O *OpenID* é um protocolo que caracteriza uma forma de um utilizador se autenticar delegando numa entidade externa (fornecedor de identidade) a validação das suas credenciais. Esta forma de autenticação permite que o utilizador use a mesma identidade em múltiplas aplicações sem a necessidade de criar, para cada uma delas, uma nova conta com a respectiva password de acesso. 

Quando um utilizador acede a uma aplicação web que utiliza esta forma de autenticação, tem que introduzir a sua identidade, previamente criada no fornecedor de identidade. A aplicação web pede ao fornecedor de identidade que verifique se o utilizador é o dono dessa identidade e, uma vez confirmada a identidade do utilizador, o fornecedor redirecciona-o para a aplicação web, enviando na *query string* a informação da identidade do utilizador. 

A informação da identidade do utilizador é pedida tirando partido da extensão ao *OpenId*, *Attribute Exchange* \ref{OpenIdAttributeExchange}. Esta extensão permite que sejam disponibilizados, pelos fornecedores de identidade, atributos que podem ser pedidos aquando do pedido de verificação da identidade de um utilizador e o acesso a ela tem que ser autorizado pelo utilizador dono da identidade. 

\label{sec:DotNetOpenAuth}

Para utilizar esta foram de autenticação foi usada a biblioteca *DotNetOpenAuth** que disponibiliza a classe *OpenIdRelyingParty*, responsável por toda a comunicação relacionada com o protocolo *OpenID*. As instâncias desta classe são parametrizáveis, permitindo escolher qual o fornecedor que se pretende usar, bem como a informação da identidade do utilizador que pretendemos obter.

Notificações
-

Existem situações em que é necessário que a aplicação comunique com os seus utilizadores, quando estes não a estão a usar. Como tal, para fazer essa comunicação, decidiu usar-se emails. 

\label{sec:Mailgun}

Os emails são enviados usando a plataforma web programável *Mailgun* \ref{mailgun} que oferece as funcionalidades de um servidor de email, como envio e recepção de emails. Além destas funcionalidades o *Mailgun* suporta o redireccionamento de emails recebidos para um URL definido pelos seus utilizadores. Toda a comunicação com o *Mailgun* é feita através de pedidos HTTP para a sua API publica, sendo necessário que a identificação do utilizador seja enviada em todos os pedidos usando uma chave única atribuída aquando do seu registo no Mailgun.

\label{sec:Configuração}

Tirando partido da funcionalidade do reencaminhamento de emails do *Mailgun* foram criadas duas *routes* \ref{mailgunroute}, que são compostas por um par de chave-valor: filtro e acção. Os filtros são compostos por funções disponibilizadas pelo *Mailgun* que indicam a componente do email que deve ser analisada: o endereço receptor da mensagem ou os cabeçalhos que lhe estão associados. Estas funções recebem como parâmetro a expressão *regex* \cite{regex} por que desejamos filtrar e, no caso de se querer filtrar por um cabeçalho, o nome do cabeçalho.

As *routes* configuradas são: 

+ Recepção de resposta a notificação

	Filtro: `match_header("references", ".*@teamworks.mailgun.org")`
	Acção:  `forward("http://http://teamworks/api/mailgun/reply")`

Quando é colocada uma nova mensagem numa discussão, os utilizadores que a seguem são notificados por email. Se responderem a esse email de notificação a sua resposta será adicionada à discussão como uma nova mensagem.

+ Criação de Discussão
	Filtro: `match_recipient("(tw\+{1}.*)@teamworks.mailgun.org")`
	Acção: `forward("http://http://teamworks/api/mailgun/create")`

Esta route é usada para que os utilizadores consigam, através do seu *token* único fazer a criação de uma nova discussão.

\label{sec:Integração}

O reencaminhamento de emails do *Mailgun* é feito através de pedidos HTTP POST enviados para as routes configuradas, sendo que a informação é enviada no corpo do pedido. Para processar e converter a informação num tipo .NET, foi criado o *model binder* *MailgunModelBinder*, que lê o corpo do pedido e transforma-o num dicionário que contém todas as chaves e os respectivos valores.

\label{sec:Notificações Assíncronas}

Uma das utilizações destas funcionalidades é a notificação dos utilizadores com as mensagens que são colocadas nas discussões a que estão associados. Para isso foi desenvolvido um mecanismo de notificação de novas mensagens que envia um email com as novas mensagens. 

Quando a aplicação é iniciada é criada uma *Thread* que, em intervalos de tempo definidos, verifica se existem mensagens para as quais ainda não foram enviadas notificações. Se existirem mensagens nessa condição, são enviados emails para os utilizadores, pela ordem pela qual foram submetidas.

Para atenuar o processamento feito pela thread de notificações o intervalo de tempo entre verificações é aumentado cada vez que não é encontrada nenhuma mensagem para enviar.
De forma a que as mensagens sejam enviados com a maior celeridade possível o intervalo entre verificações não excede os 10 minutos.

RavenDB
-

Para dar suporte à realização de *queries* complexas sobre os documentos, como operações de agregação ou pesquisas sobre campos que não são chave, o RavenDB disponibiliza um mecanismo de indexação. Os Índices são criados através da definição de funções *map-reduce*, que são aplicadas aos documentos.

\label{sec:Índices}

No decorrer deste projecto surgiu a necessidade de fazer este tipo de operações e como tal foram criados vários índices.
