# Sistema Veterinário - Console

Sistema de gestão para clínica veterinária desenvolvido em C# para console.

## Estrutura do Projeto

- **Classes de Entidade**: 15 classes representando todas as entidades do sistema (Tutor, Paciente, Veterinário, etc.)
- **CRUDs**: Operações de Create, Read, Update e Delete para cada entidade principal
- **Menus por Perfil**: 3 perfis de usuário com menus específicos (Recepcionista, Veterinário, Gerente)

## Como Executar

### Compilar todos os arquivos:

```bash
csc *.cs
```

### Executar:

```bash
./Program.exe
```

## Funcionalidades

### Perfil: Recepcionista
- Gerenciar Tutores
- Gerenciar Pacientes
- Gerenciar Agendamentos

### Perfil: Veterinário
- Consultar Agendamentos
- Gerenciar Prontuários (em construção)
- Registrar Atendimentos (em construção)
- Estoque (Medicamentos e Vacinas)

### Perfil: Gerente
- Acesso total a todas as funcionalidades
- Gerenciar Veterinários
- Gerenciar Usuários
- Estoque completo
- Relatórios (em construção)

## Dados Iniciais

O sistema já vem com dados hardcoded para teste:

**Usuários:**
- Maria Silva (Recepcionista)
- Dr. João Santos (Veterinário)
- Ana Costa (Gerente)

**Dados Cadastrados:**
- 2 Tutores
- 3 Pacientes
- 2 Veterinários

## Tecnologias

- C# Console Application
- Arquitetura baseada em CRUD
- Interface com molduras ASCII
