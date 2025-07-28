# Schedule Evaluator

Este projeto é um avaliador de escalonamentos que verifica a serializabilidade de transações concorrentes usando **timestamps** (TS).  
Ele lê um arquivo de entrada (`in.txt`) com definições de objetos de dados, transações, seus timestamps e escalonamentos, e gera um relatório (`out.txt`) indicando se cada escalonamento é válido ou se ocorreu **ROLLBACK**.

---

## ⚙️ Funcionamento

1. **Entrada (`in.txt`)**
    - Linha 1: Lista dos objetos de dados
      ```txt
      A, B, C
      ```  
    - Linha 2: Lista dos IDs das transações
      ```txt
      T1, T2, T3
      ```  
    - Linha 3: Lista dos timestamps das transações
      ```txt
      1, 4, 10
      ```  
    - Linhas seguintes: Cada escalonamento com suas operações
      ```txt
      E_1: r1(A) w2(B) c
      E_2: r2(C) w1(A) c
      ```

2. **Processamento**
    - O código inicializa objetos de dados e transações.
    - Para cada operação:
        - **Read (`r`)**: verifica conflito com o último `WriteTS`.
        - **Write (`w`)**: verifica conflito com o último `ReadTS` ou `WriteTS`.
    - Caso haja conflito → **ROLLBACK** e interrompe o escalonamento.
    - Caso contrário → Atualiza os timestamps e prossegue.

3. **Saída**
    - `out.txt`: Resultado de cada escalonamento (`OK` ou `ROLLBACK`).
    - Logs individuais para cada objeto de dado em `data_objects_log/`.

---

## 🔧 Parâmetros para Ajuste

Há apenas dois parâmetros que precisam ser ajustados.
- rootDirectory: o caminho do diretório raiz da execução, onde estará o in.txt e será gerado o out.txt e os objetos de dados.
- isLinux: caso use linux, use true. inserido apenas para permitir fácil intercalação de S.O's

No arquivo `Program.cs`:

```csharp
bool isLinux = false; // Ajuste para 'true' caso esteja executando em Linux
string rootDirectory = isLinux ? @"/home/user/tmp/schedule_evaluator/" : @"C:\temp\schedule-evaluator\";
