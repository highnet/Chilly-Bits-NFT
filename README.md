# Chilly-Bits NFT Project

This project is a Unity-based application designed to generate unique character variations (Pingu) with randomized attributes for NFT creation. It allows for batch rendering of characters and exports their metadata.

## Core Technologies

- Unity Engine
- C#

## Getting Started

## Key Components

- **[`Character.cs`](c%3A%5CUsers%5Ctelleriajo%5Ccode%5CChilly-Bits-NFT%5Csrc%5CCharacter.cs):** Defines the attributes of a character, such as hat, weapon, colors, etc.
- **[`AttributeRandomizer.cs`](c%3A%5CUsers%5Ctelleriajo%5Ccode%5CChilly-Bits-NFT%5Csrc%5CAttributeRandomizer.cs):** Manages the randomization of character attributes, handles rendering, and exports metadata to CSV files. It includes functionalities for:
  - Randomizing attributes based on rarity (common, uncommon, rare, epic, legendary).
  - Clearing and setting attributes for characters.
  - Rendering characters to PNG images.
  - Exporting attribute data to a CSV log file.
  - Batch rendering multiple character variations.

## Usage

- Configure the desired GameObjects and Materials for different rarities in the `AttributeRandomizer` component in the Unity Inspector.
- Use the UI fields provided by `batchAmountInput` and `batchStartRenderInput` in the `AttributeRandomizer` to control the generation process.
- Rendered images and CSV logs will be saved to a `renders` directory (either in `Application.dataPath` during Editor mode or `System.IO.Directory.GetCurrentDirectory()` in a build).

## License

This project is licensed under the terms of the [LICENCE.md](c%3A%5CUsers%5Ctelleriajo%5Ccode%5CChilly-Bits-NFT%5CLICENCE.md) file.

## Code of Conduct

Please read our [CODE_OF_CONDUCT.md](c%3A%5CUsers%5Ctelleriajo%5Ccode%5CChilly-Bits-NFT%5CCODE_OF_CONDUCT.md) for details on our code of conduct.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request with your changes. Ensure your code adheres to the project's coding standards and includes relevant tests if applicable.

## Support

If you encounter any issues or have questions, please open an issue on the project's GitHub repository.

## Contact

Joaquin Telleria - joaquintelleria@gmail.com

## Acknowledgements

- Unity
