# cqrs-na-pratica
Estudo sobre CQRS seguindo curso do Vladmir Khrorivok pela Pluralsight conforme [link](https://www.pluralsight.com/courses/cqrs-in-practice).

Fonte original: https://github.com/vkhorikov/CqrsInPractice

O projeto inicial foi traduzido para o português com os seguintes projetos para serem refatorados.
- API: (Projeto ASP.NET Core 2.1 que serve a UI WPF).
- Logica: Projeto do tipo Classlib que contém a lógica de dados. Utiliza FluentNhibernate como ORM.
- UI: Projeto WPF que consome API.


Ao longo do curso, é possivel aprender sobre [CQRS](https://cqrs.files.wordpress.com/2010/11/cqrs_documents.pdf) e também sobre sua implementação. Envolve outras técnicas que ajudam na programação com Domain Driven Design, Event Sourcing, estratégias de sincronização com camada anti-corrupção, bases separadas, pensamento baseado em CRUD e tarefas [(Task-based thinking)](https://cqrs.wordpress.com/documents/task-based-ui/).

Desvantagens da aplicação original

- Escalabilidade
- Performance

CRUD-based thinking vs Task-based thinking
Guidelines para implementação do CQRS
DTO vs Comandos (Quando usar? Um? O outro? Ou os dois?)

Com CQRS nosso projeto ganha:
- Simpliscidade (Código coeso)
- Performance (Ex: Permite otimizar queries para consulta)
- Escalabilidade (Ex: Em um cenário com separação de bases)
