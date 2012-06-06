Domínio
= 

A infra-estrutura expõe as funcionalidades anteriormente enunciadas através de uma aplicação web e um serviço web.

Os seguintes diagramas **use case** representam as acções que os utilizadores podem realizar na infra-estrutura. A figura **usecaseprojecto** e **usecasetarefa** representam os casos de utilização de um membro com permissões de escrita ao nível de um projecto.

<!---figure-->

![Projecto - Caso de utilização de um membro com permissão de escrita](https://www.dropbox.com/s/74grwphgl5m8me7/usecaseprojecto.png)<!--- usecaseprojecto {image} -->

![usecasetarefa]( https://www.dropbox.com/s/1se8rhskj43zt73/usecasetarefa.png)<!--- usecasetarefa -->

<!---!figure-->

A figura [usecaseprojecto](https://www.dropbox.com/s/74grwphgl5m8me7/usecaseprojecto.png "usecaseprojecto")<!---dump--> ilustra um caso de utilização de um membro de um projecto,
\textbf{sem} permissões de administração.

\begin{figure}[h]
\includegraphics[width=1\textwidth]{images\usecaseuser.png}
\caption{Projecto - Caso de utilização de um membro apenas com permissão de leitura}
\label{usecaseuser}
\end{figure}

Para solucionar o problema apresentado por este projecto foram identificadas duas entidades centrais, a \textbf{pessoa} e o \textbf{projecto}. Cada pessoa pode estar envolvida em vários projectos, assim como um projecto pode ser desenvolvido por várias pessoas. Estas entidades são descritas como:
\begin{itemize}
\item
\textbf{Pessoa}, representada pelo nome de utilizador, o \emph{email} e a \emph{password}. O \emph{email} é usado para comunicar com a pessoa e os outros dois atributos servem para autenticar o utilizado perante o sistema.
\item
O \textbf{projecto} é a raiz da infra-estrutura e agrega as pessoas que a ele estão associadas.
\end{itemize}
No contexto de um projecto é possível definir \textbf{tarefas} que, assim como a pessoa e o projecto, são consideradas entidades de domínio. Uma tarefa tem um nome e uma descrição e pode ter várias pessoas associadas. As pessoas associadas a uma tarefa podem registar o tempo que usaram na sua realização. A tarefa tem também uma previsão do tempo estimado para a sua realização (e.g. número de horas) e a data prevista de conclusão.\\

A descrição destas entidades e das suas relações é descrita no seguinte diagrama de classes:
\begin{figure}[h]
\centering
\includegraphics[scale=0.75]{uml.png}
\caption{Diagrama de classes da solução}
\label{uml}
\end{figure}
\FloatBarrier

Para a elaboração e descrição do modelo de domínio são usados conceitos de Domain-Driven Design\cite{ddd}, que podem ser consultados no Anexo I, presente no final do documento.\\

Na definição do modelo de domínio da solução foram definidos como agregados as entidades Projecto e Pessoa. O objecto de domínio Tarefa também é considerado como entidade porque possui um identificador único no sistema. O objecto de domínio Registo de Horas é definido como \emph{value object}.\\




Entidades
-

Segurança
-

