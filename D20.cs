using System;
using System.Collections.Generic;
using System.Linq;

namespace Sessao3
{
    public class Enemy
    {
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public double DamageMultiplier { get; set; }
        public string SpecialAbility { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
        public string Effect { get; set; }
        public double Value { get; set; }
    }

    public static class D20
    {
        public static void Executar()
        {
            var enemies = new List<Enemy>
            {
                new Enemy { Name = "Zumbi Corredor", MaxHealth = 100, DamageMultiplier = 1.0, SpecialAbility = "none" },
                new Enemy { Name = "Zumbi Linguarudo", MaxHealth = 100, DamageMultiplier = 1.0, SpecialAbility = "double_action" },
                new Enemy { Name = "Zumbi Infectado", MaxHealth = 100, DamageMultiplier = 1.0, SpecialAbility = "poison" },
                new Enemy { Name = "Capitão Esqueleto", MaxHealth = 150, DamageMultiplier = 1.0, SpecialAbility = "none" },
                new Enemy { Name = "Guerreiro Esqueleto", MaxHealth = 100, DamageMultiplier = 1.5, SpecialAbility = "none" },
                new Enemy { Name = "Esqueleto Atirador", MaxHealth = 100, DamageMultiplier = 1.0, SpecialAbility = "double_attack" },
                new Enemy { Name = "Mago de Água", MaxHealth = 100, DamageMultiplier = 1.0, SpecialAbility = "none" },
                new Enemy { Name = "Mago de Fogo", MaxHealth = 100, DamageMultiplier = 1.0, SpecialAbility = "extra_damage" },
                new Enemy { Name = "Mago de Raio", MaxHealth = 100, DamageMultiplier = 1.0, SpecialAbility = "paralyze" }
            };

            int danoMinimo = 5;
            int danoMaximo = 20;
            int defesaMinimo = 2;
            int defesaMaxima = 10;
            int curaMinima = 5;
            int curaMaxima = 20;

            Random random = new Random();

            int difficultyLevel = 0; // Increases with progress

            var totalDamageDealt = 0;
            var totalHealed = 0;
            var totalDamageReceived = 0;
            var enemiesDefeated = 0;
            var enemiesVictoryCount = 0;
            var defeatedEnemies = new Dictionary<string, int>();

            int playerMoney = 0;
            double playerDamageMultiplier = 1.0;
            double playerDoubleAttackChance = 0.0;
            int playerHealBonus = 0;
            double playerDamageReduction = 0.0;
            int playerMaxHealth = 100;
            double playerMoneyBonus = 0.0;
            double playerPoisonOnHitChance = 0.0;
            int playerPoisonDamagePercent = 0;
            int playerPoisonDuration = 0;
            double playerParalyzeOnHitChance = 0.0;

            var treasureItems = new List<Item>
            {
                new Item { Name = "Espada do Guerreiro", Description = "+15% de dano", Cost = 0, Effect = "damage_mult", Value = 0.15 },
                new Item { Name = "Cajado Arcano", Description = "+20% de dano", Cost = 0, Effect = "damage_mult", Value = 0.20 },
                new Item { Name = "Luvas de Ataque Duplo", Description = "+20% de chance de atacar duas vezes", Cost = 0, Effect = "double_attack", Value = 0.20 },
                new Item { Name = "Tônico de Cura Maior", Description = "+12 de cura em todas as curas", Cost = 0, Effect = "heal_bonus", Value = 12 },
                new Item { Name = "Pó Venenoso", Description = "Chance de envenenar inimigo por 2 turnos", Cost = 0, Effect = "poison_hit", Value = 0.35 },
                new Item { Name = "Amuleto de Raio", Description = "Chance de paralisar inimigo por 1 turno", Cost = 0, Effect = "paralyze_hit", Value = 0.20 },
                new Item { Name = "Escudo Reforçado", Description = "Reduz dano recebido em 12%", Cost = 0, Effect = "damage_reduction", Value = 0.12 },
                new Item { Name = "Bota Veloz", Description = "+15% de chance de atacar duas vezes", Cost = 0, Effect = "double_attack", Value = 0.15 },
                new Item { Name = "Poção de Vitalidade", Description = "+20 de vida máxima", Cost = 0, Effect = "max_health", Value = 20 },
                new Item { Name = "Pedra da Fortuna", Description = "+1 de dinheiro extra por ação", Cost = 0, Effect = "money_bonus", Value = 1 },
                new Item { Name = "Manuscrito de Combate", Description = "+25% de dano", Cost = 0, Effect = "damage_mult", Value = 0.25 }
            };

            var shopItems = new List<Item>
            {
                new Item { Name = "Adaga Venenosa", Description = "Chance de envenenar inimigo por 2 turnos", Cost = 30, Effect = "poison_hit", Value = 0.25 },
                new Item { Name = "Capa de Proteção", Description = "Reduz dano recebido em 10%", Cost = 40, Effect = "damage_reduction", Value = 0.10 },
                new Item { Name = "Elixir Maior", Description = "+10 de cura em todas as curas", Cost = 35, Effect = "heal_bonus", Value = 10 },
                new Item { Name = "Cristal de Fúria", Description = "+18% de dano", Cost = 50, Effect = "damage_mult", Value = 0.18 },
                new Item { Name = "Insígnia de Batalha", Description = "+15% de chance de atacar duas vezes", Cost = 45, Effect = "double_attack", Value = 0.15 },
                new Item { Name = "Ampola Dourada", Description = "+1 de dinheiro extra por ação", Cost = 25, Effect = "money_bonus", Value = 1 },
                new Item { Name = "Cajado de Água", Description = "+15 de cura em todas as curas", Cost = 55, Effect = "heal_bonus", Value = 15 },
                new Item { Name = "Pulseira Relâmpago", Description = "Chance de paralisar inimigo por 1 turno", Cost = 60, Effect = "paralyze_hit", Value = 0.15 },
                new Item { Name = "Armadura de Ferro", Description = "+25 de vida máxima", Cost = 65, Effect = "max_health", Value = 25 },
                new Item { Name = "Amuleto do Assassino", Description = "+12% de dano e chance de envenenar", Cost = 70, Effect = "combo_damage_poison", Value = 0.12 }
            };

            var messages = new Dictionary<int, string>
            {
                {1, "falhou miseravelmente!"},
                {2, "falhou miseravelmente!"},
                {3, "falhou!"},
                {4, "falhou!"},
                {5, "resultado mediano."},
                {6, "resultado mediano."},
                {7, "resultado bom."},
                {8, "resultado bom."},
                {9, "excelente!"},
                {10, "excelente!"},
                {11, "incrível!"},
                {12, "incrível!"},
                {13, "extraordinário!"},
                {14, "extraordinário!"},
                {15, "épico!"},
                {16, "épico!"},
                {17, "lendário!"},
                {18, "lendário!"},
                {19, "Perfeito!"},
                {20, "PERFEITO!"}
            };

            double GetMultiplicador(int d)
            {
                if (d <= 5) return 0.5;
                if (d <= 14) return 1.0;
                if (d <= 19) return 1.5;
                return 2.0;
            }

            Enemy CreateEnemyWithDifficulty(Enemy baseEnemy)
            {
                double healthMultiplier = 1.0 + (difficultyLevel * 0.1); // +10% health per difficulty level
                double damageMultiplier = 1.0 + (difficultyLevel * 0.05); // +5% damage per difficulty level

                return new Enemy
                {
                    Name = baseEnemy.Name,
                    MaxHealth = (int)(baseEnemy.MaxHealth * healthMultiplier),
                    DamageMultiplier = baseEnemy.DamageMultiplier * damageMultiplier,
                    SpecialAbility = baseEnemy.SpecialAbility
                };
            }

            List<Item> GetRandomItems(List<Item> source, int count)
            {
                return source.OrderBy(_ => random.Next()).Take(count).ToList();
            }

            void ApplyItem(Item item)
            {
                switch (item.Effect)
                {
                    case "damage_mult":
                        playerDamageMultiplier += item.Value;
                        break;
                    case "double_attack":
                        playerDoubleAttackChance += item.Value;
                        if (playerDoubleAttackChance > 0.75) playerDoubleAttackChance = 0.75;
                        break;
                    case "heal_bonus":
                        playerHealBonus += (int)item.Value;
                        break;
                    case "damage_reduction":
                        playerDamageReduction += item.Value;
                        if (playerDamageReduction > 0.5) playerDamageReduction = 0.5;
                        break;
                    case "max_health":
                        playerMaxHealth += (int)item.Value;
                        break;
                    case "money_bonus":
                        playerMoneyBonus += item.Value;
                        break;
                    case "poison_hit":
                        playerPoisonOnHitChance += item.Value;
                        if (playerPoisonOnHitChance > 0.75) playerPoisonOnHitChance = 0.75;
                        playerPoisonDamagePercent = Math.Max(playerPoisonDamagePercent, 20);
                        playerPoisonDuration = Math.Max(playerPoisonDuration, 2);
                        break;
                    case "paralyze_hit":
                        playerParalyzeOnHitChance += item.Value;
                        if (playerParalyzeOnHitChance > 0.5) playerParalyzeOnHitChance = 0.5;
                        break;
                    case "combo_damage_poison":
                        playerDamageMultiplier += item.Value;
                        playerPoisonOnHitChance += 0.20;
                        if (playerPoisonOnHitChance > 0.75) playerPoisonOnHitChance = 0.75;
                        playerPoisonDamagePercent = Math.Max(playerPoisonDamagePercent, 20);
                        playerPoisonDuration = Math.Max(playerPoisonDuration, 2);
                        break;
                }

                Console.WriteLine($"Item adquirido: {item.Name} - {item.Description}");
            }

            void EnterTreasureRoom()
            {
                Console.Clear();
                Console.WriteLine("===== SALA DO TESOURO =====\n");
                var options = GetRandomItems(treasureItems, 3);
                Console.WriteLine("Escolha um dos itens abaixo:");
                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {options[i].Name} - {options[i].Description}");
                }

                int choice;
                while (true)
                {
                    Console.Write("Digite 1, 2 ou 3 para escolher seu item: ");
                    if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= options.Count)
                    {
                        ApplyItem(options[choice - 1]);
                        break;
                    }
                }

                Console.WriteLine("\nPressione ENTER para continuar...");
                Console.ReadLine();
            }

            void EnterShop()
            {
                Console.Clear();
                Console.WriteLine("===== LOJA =====\n");
                var options = GetRandomItems(shopItems, 5);
                Console.WriteLine($"Seu dinheiro: {playerMoney}");
                Console.WriteLine("Escolha um item para comprar, ou 0 para sair sem comprar:");
                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {options[i].Name} - {options[i].Description} (Custo: {options[i].Cost})");
                }
                Console.WriteLine("0. Sair da loja");

                int choice;
                while (true)
                {
                    Console.Write("Digite o número do item que deseja comprar ou 0 para sair: ");
                    if (int.TryParse(Console.ReadLine(), out choice) && choice >= 0 && choice <= options.Count)
                    {
                        if (choice == 0)
                            break;

                        var item = options[choice - 1];
                        if (playerMoney >= item.Cost)
                        {
                            playerMoney -= item.Cost;
                            ApplyItem(item);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Você não tem dinheiro suficiente para comprar esse item.");
                        }
                    }
                }

                Console.WriteLine("\nPressione ENTER para continuar...");
                Console.ReadLine();
            }

            while (true)
            {
                Enemy selectedEnemy = CreateEnemyWithDifficulty(enemies[random.Next(enemies.Count)]);
                int vidaInimigo = selectedEnemy.MaxHealth;
                int vidaJogador = playerMaxHealth;
                int poisonTurns = 0;
                int poisonDamage = 0;
                int enemyPoisonTurns = 0;
                int enemyPoisonDamage = 0;
                bool paralyzed = false;
                bool enemyParalyzed = false;
                int rayMageCooldown = 0;

                // Enhanced enemy abilities based on difficulty
                int enhancedPoisonTurns = 2 + (difficultyLevel / 3); // More poison turns
                double enhancedPoisonDamageMultiplier = 1.0 + (difficultyLevel * 0.1); // More poison damage
                int enhancedParalyzeTurns = 1 + (difficultyLevel / 5); // More paralyze turns
                double enhancedDoubleActionChance = 20 + (difficultyLevel * 2); // Higher chance for double action
                double enhancedDoubleAttackChance = 15 + (difficultyLevel * 1.5); // Higher chance for double attack

                int battleDamageDealt = 0;
                int battleHealed = 0;
                int battleDamageReceived = 0;

                while (vidaJogador > 0 && vidaInimigo > 0)
                {
                    Console.Clear();
                    Console.WriteLine($"===== BATALHA CONTRA {selectedEnemy.Name.ToUpper()} =====\n"); Console.WriteLine($"Nível de Dificuldade: {difficultyLevel}"); Console.WriteLine($"Sua vida: {vidaJogador}/{playerMaxHealth}");
                    Console.WriteLine($"Vida do inimigo: {vidaInimigo}");
                    Console.WriteLine($"Seu dinheiro: {playerMoney}");
                    if (poisonTurns > 0) Console.WriteLine($"Poison ativo: {poisonTurns} turnos restantes, {poisonDamage} dano por turno");
                    if (enemyPoisonTurns > 0) Console.WriteLine($"Inimigo envenenado: {enemyPoisonTurns} turnos restantes, {enemyPoisonDamage} dano por turno");
                    if (paralyzed) Console.WriteLine("Você está paralisado!");
                    Console.WriteLine("-------------------");
                    Console.WriteLine("1. Atacar");
                    Console.WriteLine("2. Defender");
                    Console.WriteLine("3. Curar");

                    if (poisonTurns > 0)
                    {
                        vidaJogador -= poisonDamage;
                        battleDamageReceived += poisonDamage;
                        poisonTurns--;
                        Console.WriteLine($"Você sofreu {poisonDamage} de dano por poison!");
                    }

                    if (enemyPoisonTurns > 0)
                    {
                        vidaInimigo -= enemyPoisonDamage;
                        enemyPoisonTurns--;
                        Console.WriteLine($"O inimigo sofreu {enemyPoisonDamage} de dano por poison!");
                        if (vidaInimigo <= 0)
                            break;
                    }

                    int escolha = 0;
                    int defesaJogador = 0;
                    bool playerSkipped = false;
                    int dadoJogador = 0;
                    string messageJogador = string.Empty;
                    double multJogador = 1.0;

                    if (paralyzed)
                    {
                        paralyzed = false;
                        playerSkipped = true;
                        Console.WriteLine("Você está paralisado e perde o turno!");
                        Console.WriteLine("\nPressione ENTER para continuar...");
                        Console.ReadLine();
                    }
                    else
                    {
                        escolha = int.Parse(Console.ReadLine()!);
                        dadoJogador = random.Next(1, 21);
                        messageJogador = messages[dadoJogador];
                        multJogador = GetMultiplicador(dadoJogador);

                        if (escolha == 1)
                        {
                            int dano = (int)(random.Next(danoMinimo, danoMaximo + 1) * multJogador * playerDamageMultiplier);
                            vidaInimigo -= dano;
                            battleDamageDealt += dano;
                            playerMoney += 3 + (int)playerMoneyBonus;
                            Console.WriteLine($"Você atacou e causou {dano} de dano! (D{dadoJogador}, {messageJogador})");

                            if (playerPoisonOnHitChance > 0 && random.NextDouble() < playerPoisonOnHitChance && dano > 0)
                            {
                                enemyPoisonTurns = Math.Max(enemyPoisonTurns, playerPoisonDuration);
                                enemyPoisonDamage = Math.Max(enemyPoisonDamage, dano * playerPoisonDamagePercent / 100);
                                Console.WriteLine($"Seu ataque envenenou o inimigo! {enemyPoisonDamage} de poison por {enemyPoisonTurns} turnos.");
                            }

                            if (playerParalyzeOnHitChance > 0 && random.NextDouble() < playerParalyzeOnHitChance && vidaInimigo > 0)
                            {
                                enemyParalyzed = true;
                                Console.WriteLine("Seu ataque paralisou o inimigo por 1 turno!");
                            }

                            if (playerDoubleAttackChance > 0 && random.NextDouble() < playerDoubleAttackChance && vidaInimigo > 0)
                            {
                                int dano2 = (int)(random.Next(danoMinimo, danoMaximo + 1) * multJogador * playerDamageMultiplier);
                                vidaInimigo -= dano2;
                                battleDamageDealt += dano2;
                                playerMoney += 3 + (int)playerMoneyBonus;
                                Console.WriteLine($"Você atacou novamente e causou {dano2} de dano!");
                            }
                        }
                        else if (escolha == 2)
                        {
                            defesaJogador = (int)(random.Next(defesaMinimo, defesaMaxima + 1) * multJogador);
                            playerMoney += 2 + (int)playerMoneyBonus;
                            Console.WriteLine($"Você se preparou e vai bloquear {defesaJogador} de dano! (D{dadoJogador}, {messageJogador})");
                        }
                        else if (escolha == 3)
                        {
                            int cura = (int)(random.Next(curaMinima, curaMaxima + 1) * multJogador) + playerHealBonus;
                            vidaJogador += cura;
                            if (vidaJogador > playerMaxHealth) vidaJogador = playerMaxHealth;
                            battleHealed += cura;
                            playerMoney += 1 + (int)playerMoneyBonus;
                            Console.WriteLine($"Você se curou em {cura} de vida! (D{dadoJogador}, {messageJogador})");
                        }
                    }

                    if (!playerSkipped && vidaInimigo <= 0)
                        break;

                    if (vidaInimigo <= 0)
                        break;

                    if (rayMageCooldown > 0) rayMageCooldown--;

                    if (enemyParalyzed)
                    {
                        Console.WriteLine("O inimigo está paralisado e perde o turno!");
                        enemyParalyzed = false;
                        Console.WriteLine("\nPressione ENTER para continuar...");
                        Console.ReadLine();
                        continue;
                    }

                    int escolhaInimigo;
                    if (vidaInimigo <= 30)
                    {
                        escolhaInimigo = random.Next(1, 4);
                    }
                    else
                    {
                        escolhaInimigo = random.Next(1, 3);
                    }

                    int dadoInimigo = random.Next(1, 21);
                    string messageInimigo = messages[dadoInimigo];
                    double multInimigo = GetMultiplicador(dadoInimigo);

                    bool doubleAction = false;
                    if (selectedEnemy.SpecialAbility == "double_action" && random.Next(100) < enhancedDoubleActionChance)
                    {
                        doubleAction = true;
                    }

                    bool doubleAttack = false;
                    if (selectedEnemy.SpecialAbility == "double_attack" && random.Next(100) < enhancedDoubleAttackChance)
                    {
                        doubleAttack = true;
                    }

                    for (int action = 0; action < (doubleAction ? 2 : 1); action++)
                    {
                        if (escolhaInimigo == 1)
                        {
                            int dano = (int)(random.Next(danoMinimo, danoMaximo + 1) * multInimigo * selectedEnemy.DamageMultiplier);
                            if (selectedEnemy.SpecialAbility == "extra_damage")
                            {
                                dano += dano / 4;
                            }
                            dano -= defesaJogador;
                            if (dano < 0) dano = 0;
                            dano = (int)(dano * (1 - playerDamageReduction));
                            vidaJogador -= dano;
                            battleDamageReceived += dano;
                            Console.WriteLine($"O inimigo te atacou e causou {dano} de dano! (D{dadoInimigo}, {messageInimigo})");
                            if (selectedEnemy.SpecialAbility == "poison" && dano > 0)
                            {
                                poisonTurns = enhancedPoisonTurns;
                                poisonDamage = (int)((dano / 6.0) * enhancedPoisonDamageMultiplier);
                                Console.WriteLine($"Você foi envenenado! Receberá {poisonDamage} de dano por {poisonTurns} turnos.");
                            }
                            if (selectedEnemy.SpecialAbility == "paralyze" && rayMageCooldown == 0 && random.Next(100) < 50)
                            {
                                paralyzed = true;
                                rayMageCooldown = 5;
                                Console.WriteLine($"Você foi paralisado por {enhancedParalyzeTurns} turno{(enhancedParalyzeTurns > 1 ? "s" : "")}!");
                            }
                            if (doubleAttack && action == 0)
                            {
                                int dano2 = (int)(random.Next(danoMinimo, danoMaximo + 1) * multInimigo * selectedEnemy.DamageMultiplier);
                                dano2 -= defesaJogador;
                                if (dano2 < 0) dano2 = 0;
                                dano2 = (int)(dano2 * (1 - playerDamageReduction));
                                vidaJogador -= dano2;
                                battleDamageReceived += dano2;
                                Console.WriteLine($"O inimigo atacou novamente e causou {dano2} de dano!");
                            }
                        }
                        else if (escolhaInimigo == 2)
                        {
                            Console.WriteLine($"O inimigo se defendeu e se prepara para reduzir seu próximo ataque. (D{dadoInimigo}, {messageInimigo})");
                        }
                        else if (escolhaInimigo == 3)
                        {
                            int cura = (int)(random.Next(curaMinima, curaMaxima + 1) * multInimigo);
                            vidaInimigo += cura;
                            if (vidaInimigo > selectedEnemy.MaxHealth) vidaInimigo = selectedEnemy.MaxHealth;
                            Console.WriteLine($"O inimigo se curou em {cura}! (D{dadoInimigo}, {messageInimigo})");
                        }

                        if (action == 0 && doubleAction)
                        {
                            escolhaInimigo = random.Next(1, 4);
                            dadoInimigo = random.Next(1, 21);
                            messageInimigo = messages[dadoInimigo];
                            multInimigo = GetMultiplicador(dadoInimigo);
                        }
                    }

                    Console.WriteLine("\nPressione ENTER para continuar...");
                    Console.ReadLine();
                }

                Console.Clear();
                bool playerWon = vidaJogador > 0;
                if (playerWon)
                {
                    enemiesDefeated++;
                    if (!defeatedEnemies.ContainsKey(selectedEnemy.Name))
                        defeatedEnemies[selectedEnemy.Name] = 0;
                    defeatedEnemies[selectedEnemy.Name]++;

                    // Increase difficulty every 5 enemies defeated
                    if (enemiesDefeated % 5 == 0)
                    {
                        difficultyLevel++;
                        Console.WriteLine($"Parabéns! Você alcançou o nível de dificuldade {difficultyLevel}!");
                        Console.WriteLine("Os inimigos estão ficando mais fortes...");
                    }

                    Console.WriteLine($"Você venceu o {selectedEnemy.Name}!");
                }
                else
                {
                    enemiesVictoryCount++;
                    Console.WriteLine($"Você foi derrotado pelo {selectedEnemy.Name}...");
                }

                totalDamageDealt += battleDamageDealt;
                totalHealed += battleHealed;
                totalDamageReceived += battleDamageReceived;

                Console.WriteLine($"Dano causado neste combate: {battleDamageDealt}");
                Console.WriteLine($"Total curado neste combate: {battleHealed}");
                Console.WriteLine($"Dano recebido neste combate: {battleDamageReceived}");

                if (playerWon)
                {
                    Console.WriteLine("\nVocê encontrou uma sala do tesouro e precisa entrar nela.");
                    Console.WriteLine("Pressione ENTER para entrar na sala do tesouro...");
                    Console.ReadLine();
                    EnterTreasureRoom();

                    if (enemiesDefeated % 2 == 0)
                    {
                        Console.WriteLine("\nUma loja apareceu após sua vitória. Você deve entrar nela.");
                        Console.WriteLine("Pressione ENTER para entrar na loja...");
                        Console.ReadLine();
                        EnterShop();
                    }
                }

                Console.Write("Deseja jogar outra batalha? (s/n): ");
                var continuar = Console.ReadLine()?.Trim().ToLower();
                if (continuar != "s" && continuar != "sim")
                    break;
            }

            Console.Clear();
            Console.WriteLine("===== RESUMO DE TODAS AS PARTIDAS =====\n");
            Console.WriteLine("Inimigos derrotados:");
            Console.WriteLine("Nome do inimigo           | Derrotas");
            Console.WriteLine("--------------------------|---------");
            if (defeatedEnemies.Count == 0)
            {
                Console.WriteLine("Nenhum inimigo derrotado.");
            }
            else
            {
                foreach (var kv in defeatedEnemies)
                {
                    Console.WriteLine($"{kv.Key.PadRight(25)} | {kv.Value}");
                }
            }

            Console.WriteLine($"\nDano total causado: {totalDamageDealt}");
            Console.WriteLine($"Total curado: {totalHealed}");
            Console.WriteLine($"Dano recebido: {totalDamageReceived}");
            Console.WriteLine($"Inimigos derrotados: {enemiesDefeated}");
            Console.WriteLine($"Inimigos que te venceram: {enemiesVictoryCount}");
            Console.WriteLine($"Dinheiro total final: {playerMoney}");
        }
    }
}