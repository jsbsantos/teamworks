Serviços
=

\label{sec:servicos}

Os serviços disponibilizados pela infra-estrutura encontram-se no *namespace* `Teamworks.Core` e incluem serviços de acesso a dados, segurança, autenticação e envio de notificações.

Acesso a Dados
-

\label{acesso-a-dados}

Para fazer o acesso a dados persistidos em bases de dados, tipicamente é utilizado o padrão *Repository* \cite[pp.~322-327]{patterns}. Este padrão encapsula a lógica de persistência dos objectos de domínio de forma a desacoplar o modelo de domínio da lógica de acesso a dados. Na solução implementada também é usada esta abordagem, através da utilização do cliente *Raven* como um repositório.

A inicialização e configuração do cliente (`IDocumentStore`) está presente na propriedade `Database` da classe `Global` e a sua inicialização deve ser feita no início da aplicação. 


Segurança
- 

\label{seguranca}

A plataforma impõe as políticas de acesso em conjunto com o *Authorization bundle*. A validação da autorização é feita quando se tenta aceder a um documento. Se um utilizador tentar aceder a um documento e não tiver permissões para o fazer é lançada uma excepção.

A configuração do cliente (`IDocumentStore`) para que este valide se é possível interagir com o documento, de acordo com o utilizador, é feita utilizando métodos de extensão presentes no *assembly* `Raven.Client.Authorization.dll`.

###OpenID

\label{sec:OpenID}

O *OpenID* \cite{openid} é um protocolo que caracteriza uma forma de autenticação, delegando numa entidade externa (fornecedor de identidade) a validação das credenciais. Esta forma de autenticação permite que o utilizador use a mesma identidade em múltiplas aplicações sem a necessidade de criar, para cada uma delas, uma nova conta com a respectiva password de acesso. 

Quando um utilizador acede a uma aplicação web que utiliza esta forma de autenticação tipicamente tem que introduzir a sua identidade, previamente criada no fornecedor de identidade. A aplicação web pede ao fornecedor de identidade que verifique se o utilizador é o dono dessa identidade e, em caso de sucesso, redirecciona-o para a aplicação web, enviando na *query string* a informação da identidade do utilizador. 

A informação da identidade do utilizador é obtida tirando partido da extensão ao *OpenId*, *Attribute Exchange* \cite{openidattributeexchange}.
Esta extensão permite que sejam disponibilizados, pelos fornecedores de identidade, atributos que podem ser pedidos aquando do pedido de verificação da identidade, no entanto o acesso a esta informação tem que ser autorizado pelo utilizador dono da identidade. 

\label{sec:DotNetOpenAuth}

Para utilizar esta foram de autenticação foi usada a biblioteca *DotNetOpenAuth* \cite{dotnetopenauth} que disponibiliza a classe `OpenIdRelyingParty`, responsável por toda a comunicação relacionada com o protocolo *OpenID*. As instâncias desta classe são parametrizáveis, permitindo escolher qual o fornecedor que se pretende usar, bem como a informação da identidade do utilizador que pretendemos obter.

Notificações
-

Existem situações em que a aplicação necessita de comunicar com os seus utilizadores. Como tal, decidiu usar-se o email que é a forma mais comum neste tipo de aplicações.

\label{sec:Mailgun}

Os *emails* são enviados usando a plataforma web programável *Mailgun* \cite{mailgun} que oferece as funcionalidades de um servidor de email, como envio e recepção de emails. Além destas funcionalidades o *Mailgun* suporta o redireccionamento de emails recebidos para um URL configurável. Toda a comunicação com o *Mailgun* é feita através de pedidos HTTP para a sua Api pública, sendo necessário que a identificação do utilizador seja enviada em todos os pedidos usando uma chave única atribuída aquando do seu registo no *Mailgun*.

\label{sec:Configuração}

Tirando partido da funcionalidade do reencaminhamento de emails foram criadas duas *routes*, que são compostas por um par chave-valor: filtro e acção. Os filtros são compostos por funções disponibilizadas pelo *Mailgun* que indicam a componente do email que deve ser analisada: o endereço receptor da mensagem ou os cabeçalhos que lhe estão associados. Estas funções recebem como parâmetro uma expressão *regex*[^regex] por que desejamos filtrar e, no caso de se querer filtrar por um cabeçalho, o nome do cabeçalho.

As *routes* configuradas são: 

+ Recepção de resposta a notificação

	Filtro: `match`_`header("references", ".*@teamworks.mailgun.org")`
	
	Acção: `forward("http://http://teamworks/api/mailgun/reply")`

Quando é colocada uma nova mensagem numa debate, os utilizadores que a seguem são notificados por email. Se responderem a esse email a sua resposta é adicionada ao debate como uma nova mensagem.

+ Criação de Debate

	Filtro: `match`_`recipient("(tw\+{1}.*)@teamworks.mailgun.org")`
	
	Acção: `forward("http://teamworks/api/mailgun/create")`

Esta route é usada para que os utilizadores consigam, através do seu *token* único que lhes é atribuido, fazer a criação de uma nova debate.

\label{sec:Integração}

O reencaminhamento de emails do *Mailgun* é feito através de pedidos HTTP POST enviados para os *endpoints* configuradas, sendo a informação enviada no corpo do pedido. Para processar e converter a informação num tipo .NET, foi criado o *model binder* `MailgunModelBinder`, que lê o corpo do pedido e transforma-o num dicionário.

\label{sec:Notificações Assíncronas}

Uma das utilizações destas funcionalidades é a notificação dos utilizadores com as mensagens que são colocadas nas debates a que estão associados. Para isso foi desenvolvido um mecanismo de notificação que envia um email com as novas mensagens. 

Quando a aplicação é iniciada é criada uma *thread* que, em intervalos de tempo definidos, verifica se existem mensagens para as quais ainda não foram enviadas notificações. Se existirem mensagens nessa condição, são enviados emails para os utilizadores, pela ordem pela qual foram submetidas.

Para atenuar o processamento feito pela *thread* de notificações o intervalo de tempo entre verificações é aumentado cada vez que não é encontrada nenhuma mensagem para enviar.
Para que as mensagens sejam enviados com a maior celeridade possível o intervalo entre verificações não excede os 10 minutos.

RavenDB
-

Para dar suporte à realização de *queries* complexas sobre os documentos, como operações de agregação ou pesquisas sobre campos que não são chave, o *Raven* disponibiliza um mecanismo de indexação. Os Índices são criados através da definição de funções *map-reduce* \cite{mapreduce}, que são aplicadas aos documentos.

\label{sec:Índices}

No decorrer deste projecto surgiu a necessidade de fazer este tipo de operações e como tal foram criados vários índices.

O índice `DiscussionMessagesPendingNotification` foi criado para agregar a informação de debates e respectivas mensagens e obter da informação relevante para o envio de notificações por email. Além disso este índice permite filtrar as mensagens para as quais ainda não foram enviadas notificações.

Para obter o número de actividades e debates associados a projectos foi criado o índice `ProjectEntityCount`. Este índice agrega a informação de projectos, debates e actividades retornando as contagens enunciadas anteriormente para cada um dos projectos.

A obtenção das entidades associadas a um projecto é feita através da utilização do índice `ProjectsEntitiesRelated`.

O índice `Timelog`_`Filter` agrega a informação de registo de horas e da respectiva entidade, possibilitando a filtragem de resultados por actividade, utilizador e datas.

Para obter a informação sobre as actividades, a sua duração e datas de início é usado o índice `ActivitiesDuration`.

[^regex]: Regular Expression
