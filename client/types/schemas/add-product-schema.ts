import { z } from 'zod';

const allowedImageTypes = new Set(['image/jpeg', 'image/jpg', 'image/png', 'image/webp']);
const maxImageSize = 5 * 1024 * 1024;

export const AddProductSchema = z.object({
  title: z.string().min(3, 'Title is too short').max(100, 'Maximum 100 characters available'),
  description: z
    .string()
    .refine((val) => val === '' || val === null || val === undefined || val.length >= 3, {
      message: 'Description is too short',
    })
    .max(255, 'Maximum 255 characters available')
    .optional()
    .nullish(),
  image: z
    .any()
    .refine(
      (file) => {
        const actualFile = file instanceof FileList ? file[0] : file;

        return actualFile instanceof File && allowedImageTypes.has(actualFile.type);
      },
      { message: 'Invalid file type. Only jpg, png and WebP formats allowed' },
    )
    .refine((file) => file == undefined || (file instanceof File && file.size <= maxImageSize), {
      message: 'File is too big. Max size of 5 mb permitted',
    }),
});
export type AddProduct = z.infer<typeof AddProductSchema>;
