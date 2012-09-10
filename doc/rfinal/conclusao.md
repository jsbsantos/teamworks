Conclusão
=

A solução desenvolvida permite aos utilizadores fazer diversas acções sobre a plataforma, como a criação e gestão de projectos, tarefas e debates. No entanto ainda existe trabalho que é necessário fazer para que a plataforma atinja os níveis a que nos propusemos.

No desenvolvimento deste projecto foram usadas técnicas e tecnologias com as quais não estávamos familiarizados, havendo por isso um período de aprendizagem. Como exemplo temos as bases de dados de documentos, *domain driven design*, *RavenDB*, *knockout* e *web Api*.

Este desconhecimento inicial, principalmente das bases de dados de documentos, *domain driven design* e *RavenDB*, condicionou o desenvolvimento do modelo de dados, tornando-o um processo iterativo. Durante este processo, e com a aprendizagem feita sobre estes temas, o modelo de dados foi alterado até chegar ao estado actual.

Trabalho Futuro
-

A versão actual da plataforma permite a edição dos seus dados de uma forma muito básica. 
Até à data de entrega da versão final pretende-se implementar funcionalidades que complementem as que já estão disponíveis. 
As funcionalidades a implementar são:

 * Autorização 

	No decorrer do projecto foi detectado um erro na implementação do *bundle* de autorização disponibilizado pelo RavenDB[^ravendberro] o erro já foi corrigido.

 * Planeamento de tarefas

	Adicionar à gestão de tarefas a possibilidade de lhes atribuir prioridades e que dependam umas das outras. Desta forma é melhorado o controlo e organização das tarefas de um projecto.

 * Análise e Monitorização 

	Pretende-se implementar indicadores para análise de informação sobre o projecto, as suas tarefas e utilizadores para que o responsável por um projecto seja capaz de avaliar qual o estado em que este se encontra.

 * *Dashboard*

	Deverá estar disponível aos utilizadores informação sobre quais os projectos e respectivas tarefas que lhe estão atribuídas para um período de tempo (e.g. semana actual).

Além de novas funcionalidades existem aspectos da solução que têm de ser melhorados. 
A cobertura dos testes unitários deve ser alargada tanto na Api, como na aplicação web e em todos os serviços.

A forma de autenticação usada na api web é pouco segura por isso devem ser estudadas alternativas, como a utilização do esquema HMAC (Hash Message Authentication Code) ou Oauth.

[^ravendberro]: O *topic* http://goo.gl/FpIoq no Google Groups do RavenDB detalha o erro e a solução.


Conclusão
Aprendizagem de novos conceitos e tecnologias
Estudadas novas frameworks e apis não leccionadas no actual plano de estudos ... 
Foi utilizada uma versão "beta" da web api
Nem todas as funcionalidades implementadas têm o grau de "qualidade" esperado

Notas a salientar:

lucidcharts: licensa
ravendb: ravenhq para configuração da base de dados remota
mailgun: entramos em contacto para configuração
utilização de frameworks opensource
problemas (bug) com frameworks utilizadas

Trabalho futuro:
monitorização e análise de projectos
repositório de ficheiros
